using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Shared.Responses;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Commands.Orders.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, BaseResponse<int>>
{
    private readonly IProductRepository _productRepository;
    private readonly IShippingOptionRepository _shippingOptionRepository;
    private readonly IPriceCalculatorService _priceCalculatorService;
    private readonly IOrderRepository _orderRepository;

    public CreateOrderCommandHandler(
        IProductRepository productRepository,
        IShippingOptionRepository shippingOptionRepository,
        IPriceCalculatorService priceCalculatorService,
        IOrderRepository orderRepository)
    {
        _productRepository = productRepository;
        _shippingOptionRepository = shippingOptionRepository;
        _priceCalculatorService = priceCalculatorService;
        _orderRepository = orderRepository;
    }

    public async Task<BaseResponse<int>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
        {
            return BaseResponse<int>.Fail(
                "Order creation failed",
                new List<string> { "Product not found" }
            );
        }

        var shippingOption = await _shippingOptionRepository.GetByIdAsync(request.ShippingOptionId, cancellationToken);
        if (shippingOption is null)
        {
            return BaseResponse<int>.Fail(
                "Order creation failed",
                new List<string> { "Shipping option not found" }
            );
        }

        if (!shippingOption.IsActive)
        {
            return BaseResponse<int>.Fail(
                "Order creation failed",
                new List<string> { "Selected shipping option is inactive" }
            );
        }

        if (shippingOption.OriginCountry != product.OriginCountry)
        {
            return BaseResponse<int>.Fail(
                "Order creation failed",
                new List<string> { "Shipping option does not match product origin country" }
            );
        }

        var calculation = await _priceCalculatorService
        .CalculateForOptionAsync(product, shippingOption);

        if (calculation is null)
        {
            return BaseResponse<int>.Fail(
                "Order creation failed",
                new List<string> { "Could not calculate price for selected shipping option" }
            );
        }

        var order = new Order
        {
            ProductId = product.Id,
            ShippingOptionId = shippingOption.Id,
            ProductPrice = calculation.ProductPrice,
            ShippingCost = calculation.ShippingCost,
            CustomsFee = calculation.CustomsFee,
            WarehouseFee = calculation.WarehouseFee,
            LocalDeliveryFee = calculation.LocalDeliveryFee,
            FinalPrice = calculation.FinalPrice,
            Status = OrderStatus.Pending
        };

        await _orderRepository.AddAsync(order, cancellationToken);
        await _orderRepository.SaveChangesAsync(cancellationToken);

        return BaseResponse<int>.Ok(order.Id, "Order created successfully");
    }
}