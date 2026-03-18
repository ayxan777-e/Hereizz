using Application.DTOs.Orders;
using Application.Shared.Responses;
using MediatR;

namespace Application.Queries.Orders.GetAllOrders;

public class GetOrdersQuery
    : IRequest<BaseResponse<PagedResponse<List<OrderListItemDto>>>>
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? Status { get; set; }

    public string? SearchTerm { get; set; }
}