using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Shared.Responses;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.Orders.Checkout;

public class CheckoutCommandHandler : IRequestHandler<CheckoutCommand, BaseResponse<int>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly INotificationService _notificationService;
    private readonly IEmailService _emailService;
    private readonly UserManager<User> _userManager;
    private readonly IEmailTemplateService _emailTemplateService;

    public CheckoutCommandHandler(
        ICartRepository cartRepository,
        IOrderRepository orderRepository,
        ICurrentUserService currentUserService,
        INotificationService notificationService,
        IEmailService emailService,
        UserManager<User> userManager,
        IEmailTemplateService emailTemplateService)
    {
        _cartRepository = cartRepository;
        _orderRepository = orderRepository;
        _currentUserService = currentUserService;
        _notificationService = notificationService;
        _emailService = emailService;
        _userManager = userManager;
        _emailTemplateService = emailTemplateService;
    }

    public async Task<BaseResponse<int>> Handle(CheckoutCommand request, CancellationToken ct)
    {
        if (!_currentUserService.IsAuthenticated)
        {
            return BaseResponse<int>.Fail(
                "Unauthorized",
                new List<string> { "User is not authenticated." },
                ErrorType.Unauthorized);
        }

        var userId = _currentUserService.UserId;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return BaseResponse<int>.Fail(
                "Unauthorized",
                new List<string> { "User ID not found in token." },
                ErrorType.Unauthorized);
        }

        var cart = await _cartRepository.GetByUserIdWithItemsAsync(userId, ct);

        if (cart is null || cart.Items is null || !cart.Items.Any())
        {
            return BaseResponse<int>.Fail(
                "Checkout failed",
                new List<string> { "Cart is empty." },
                ErrorType.BadRequest);
        }
        var inactiveProducts = cart.Items
                   .Where(x => x.Product is null || !x.Product.IsActive)
                   .Select(x => x.Product != null ? x.Product.Title : $"ProductId: {x.ProductId}")
                   .Distinct()
                   .ToList();

        if (inactiveProducts.Any())
        {
            return BaseResponse<int>.Fail(
                "Checkout failed",
                new List<string>
                {
            $"Some products in your cart are no longer available: {string.Join(", ", inactiveProducts)}"
                },
                ErrorType.BadRequest);
        }

        var order = new Order
        {
            UserId = userId,
            Status = OrderStatus.Pending,
            Items = cart.Items.Select(x => new OrderItem
            {
                ProductId = x.ProductId,
                ProductTitle = x.Product.Title,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                ShippingOptionId = x.ShippingOptionId,
                ShippingOptionName = x.ShippingOptionName,
                ShippingCost = x.ShippingCost,
                CustomsFee = x.CustomsFee,
                WarehouseFee = x.WarehouseFee,
                LocalDeliveryFee = x.LocalDeliveryFee,
                FinalPrice = x.FinalPrice,
                TransportType = x.TransportType.ToString(),
                EstimatedMinDays = x.EstimatedMinDays,
                EstimatedMaxDays = x.EstimatedMaxDays
            }).ToList()
        };

        order.TotalPrice = order.Items.Sum(x => x.FinalPrice * x.Quantity);
        order.Payment = new Payment
        {
            UserId = userId,
            Amount = order.TotalPrice,
            Method = PaymentMethod.Card,
            Status = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _orderRepository.AddAsync(order, ct);
        await _cartRepository.ClearCartAsync(cart.Id, ct);
        await _orderRepository.SaveChangesAsync(ct);


        var productNames = order.Items
            .Select(x => x.ProductTitle)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct()
            .ToList();

        var productsText = productNames.Any()
            ? string.Join(", ", productNames)
            : "your selected products";

        await _notificationService.CreateAsync(
            userId,
            "Your order has been created",
            $"Your order for {productsText} has been created successfully.",
            NotificationType.OrderCreated,
            ct);

        var user = await _userManager.FindByIdAsync(userId);

        if (!string.IsNullOrWhiteSpace(user?.Email))
        {
            var subject = "Your order has been created";
            var body = _emailTemplateService.BuildOrderCreatedTemplate(productsText, order.Status);

            await _emailService.SendEmailAsync(user.Email, subject, body, ct);
        }

        return BaseResponse<int>.Ok(order.Id, "Checkout completed successfully");
    }
}