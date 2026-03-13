using Application.DTOs.Orders;
using Application.Interfaces.Repositories;
using Application.Shared.Responses;
using MediatR;

namespace Application.Queries.Orders.GetOrders;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, BaseResponse<List<OrderListItemDto>>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<BaseResponse<List<OrderListItemDto>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetAllWithDetailsAsync(cancellationToken);

        var data = orders.Select(x => new OrderListItemDto
        {
            Id = x.Id,
            ProductId = x.ProductId,
            ProductTitle = x.Product.Title,
            ShippingOptionId = x.ShippingOptionId,
            ShippingOptionName = x.ShippingOption.Name,
            FinalPrice = x.FinalPrice,
            Status = x.Status.ToString(),
            CreatedAt = x.CreatedAt
        }).ToList();

        return BaseResponse<List<OrderListItemDto>>.Ok(data, "Orders fetched successfully");
    }
}