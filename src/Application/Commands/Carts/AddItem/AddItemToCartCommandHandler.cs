using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Shared.Responses;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Commands.Cart.AddItem;

public class AddItemToCartCommandHandler : IRequestHandler<AddItemToCartCommand, BaseResponse>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IShippingOptionRepository _shippingOptionRepository;
    private readonly IPriceCalculatorService _priceCalculatorService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AddItemToCartCommandHandler(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IShippingOptionRepository shippingOptionRepository,
        IPriceCalculatorService priceCalculatorService,
        IHttpContextAccessor httpContextAccessor)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _shippingOptionRepository = shippingOptionRepository;
        _priceCalculatorService = priceCalculatorService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<BaseResponse> Handle(AddItemToCartCommand request, CancellationToken ct)
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user is null)
        {
            return BaseResponse.Fail(
                "Unauthorized",
                new List<string> { "User context is missing." },
                ErrorType.Unauthorized);
        }

        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return BaseResponse.Fail(
                "Unauthorized",
                new List<string> { "User ID not found in token." },
                ErrorType.Unauthorized);
        }

        var product = await _productRepository.GetByIdAsync(request.ProductId, ct);
        if (product is null)
        {
            return BaseResponse.Fail(
                "Product not found",
                new List<string> { "The selected product does not exist." },
                ErrorType.NotFound);
        }

        var shippingOption = await _shippingOptionRepository.GetByIdAsync(request.ShippingOptionId, ct);
        if (shippingOption is null)
        {
            return BaseResponse.Fail(
                "Shipping option not found",
                new List<string> { "The selected shipping option does not exist." },
                ErrorType.NotFound);
        }

        var calculations = await _priceCalculatorService.CalculateAsync(request.ProductId, ct);

        var selectedCalculation = calculations
            .FirstOrDefault(x => x.ShippingOptionId == shippingOption.Id);

        if (selectedCalculation is null)
        {
            return BaseResponse.Fail(
                "Calculation failed",
                new List<string> { "Could not calculate the selected shipping option." },
                ErrorType.BadRequest);
        }

        var cart = await _cartRepository.GetByUserIdWithItemsAsync(userId, ct);

        if (cart is null)
        {
            cart = new Domain.Entities.Cart
            {
                UserId = userId
            };

            await _cartRepository.AddAsync(cart, ct);
            await _cartRepository.SaveChangesAsync(ct);
        }

        var existingItem = cart.Items.FirstOrDefault(x =>
            x.ProductId == request.ProductId &&
            x.ShippingOptionId == request.ShippingOptionId);

        if (existingItem is not null)
        {
            existingItem.Quantity += request.Quantity;
        }
        else
        {
            var cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = product.Id,
                Quantity = request.Quantity,

                ShippingOptionId = shippingOption.Id,

                UnitPrice = selectedCalculation.ProductPrice,
                ShippingCost = selectedCalculation.ShippingCost,
                CustomsFee = selectedCalculation.CustomsFee,
                WarehouseFee = selectedCalculation.WarehouseFee,
                LocalDeliveryFee = selectedCalculation.LocalDeliveryFee,
                FinalPrice = selectedCalculation.FinalPrice,

                ShippingOptionName = selectedCalculation.ShippingOptionName,
                TransportType = selectedCalculation.TransportType,
                EstimatedMinDays = selectedCalculation.EstimatedMinDays,
                EstimatedMaxDays = selectedCalculation.EstimatedMaxDays
            };

            cart.Items.Add(cartItem);
        }

        await _cartRepository.SaveChangesAsync(ct);

        return BaseResponse.Ok("Item added to cart successfully");
    }
}