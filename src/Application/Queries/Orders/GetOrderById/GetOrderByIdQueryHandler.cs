using Application.DTOs.Orders;
using Application.Interfaces.Repositories;
using Application.Shared.Responses;
using MediatR;

namespace Application.Queries.Orders.GetOrderById;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, BaseResponse<OrderDetailsDto>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<BaseResponse<OrderDetailsDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);

        if (order is null)
        {
            return BaseResponse<OrderDetailsDto>.Fail(
                "Order not found",
                new List<string> { "Order with given id does not exist" },
                ErrorType.NotFound
            );
        }

        var dto = new OrderDetailsDto
        {
            Id = order.Id,
            ProductId = order.ProductId,
            ProductTitle = order.Product.Title,
            ShippingOptionId = order.ShippingOptionId,
            ShippingOptionName = order.ShippingOption.Name,
            ProductPrice = order.ProductPrice,
            ShippingCost = order.ShippingCost,
            CustomsFee = order.CustomsFee,
            WarehouseFee = order.WarehouseFee,
            LocalDeliveryFee = order.LocalDeliveryFee,
            FinalPrice = order.FinalPrice,
            Status = order.Status.ToString(),
            CreatedAt = order.CreatedAt
        };

        return BaseResponse<OrderDetailsDto>.Ok(dto, "Order fetched successfully");
    }
}