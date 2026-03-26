using Application.DTOs.Orders;
using Application.Interfaces.Repositories;
using Application.Shared.Responses;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Orders.GetAllOrders;

public class GetOrdersQueryHandler
    : IRequestHandler<GetOrdersQuery, BaseResponse<PagedResponse<List<OrderListItemDto>>>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<BaseResponse<PagedResponse<List<OrderListItemDto>>>> Handle(
        GetOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var query = _orderRepository.GetQueryableWithDetails();

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (Enum.TryParse<OrderStatus>(request.Status, true, out var parsedStatus))
            {
                query = query.Where(x => x.Status == parsedStatus);
            }
            else
            {
                return BaseResponse<PagedResponse<List<OrderListItemDto>>>.Fail(
                    "Invalid status",
                    new List<string> { "Provided status value is not valid." },
                    ErrorType.BadRequest
                );
            }
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();

            query = query.Where(x =>
                x.Items.Any(i => i.ProductTitle.ToLower().Contains(searchTerm)));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var orders = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new OrderListItemDto
            {
                Id = x.Id,
                ItemCount = x.Items.Sum(i => i.Quantity),
                TotalPrice = x.TotalPrice,
                Status = x.Status.ToString(),
                CreatedAt = x.CreatedAt
            })
            .ToListAsync(cancellationToken);

        var response = new PagedResponse<List<OrderListItemDto>>
        {
            Items = orders,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
        };

        return BaseResponse<PagedResponse<List<OrderListItemDto>>>
            .Ok(response, "Orders fetched successfully");
    }
}