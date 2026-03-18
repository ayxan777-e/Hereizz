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

        // 🔍 Status filter
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

        // 🔍 Search filter
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(x => x.Product.Title.ToLower().Contains(request.SearchTerm.ToLower()));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var orders = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new OrderListItemDto
            {
                Id = x.Id,
                ProductId = x.ProductId,
                ProductTitle = x.Product.Title,
                ShippingOptionId = x.ShippingOptionId,
                ShippingOptionName = x.ShippingOption.Name,
                FinalPrice = x.FinalPrice,
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