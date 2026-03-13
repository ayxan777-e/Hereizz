using Application.DTOs.Orders;
using Application.Shared.Responses;
using MediatR;

namespace Application.Queries.Orders.GetOrders;

public class GetOrdersQuery : IRequest<BaseResponse<List<OrderListItemDto>>>
{
}