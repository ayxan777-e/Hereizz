using Application.DTOs.Orders;
using Application.Interfaces.Repositories;
using Application.Shared.Responses;
using Domain.Constants;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Queries.Orders.GetAllOrders;

public class GetOrdersQueryHandler
    : IRequestHandler<GetOrdersQuery, BaseResponse<PagedResponse<List<OrderListItemDto>>>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetOrdersQueryHandler(
        IOrderRepository orderRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _orderRepository = orderRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<BaseResponse<PagedResponse<List<OrderListItemDto>>>> Handle(
        GetOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user is null)
        {
            return BaseResponse<PagedResponse<List<OrderListItemDto>>>.Fail(
                "Unauthorized",
                new List<string> { "User context is missing." },
                ErrorType.Unauthorized);
        }

        var currentUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(currentUserId))
        {
            return BaseResponse<PagedResponse<List<OrderListItemDto>>>.Fail(
                "Unauthorized",
                new List<string> { "User ID not found in token." },
                ErrorType.Unauthorized);
        }

        var isAdmin = user.IsInRole(Roles.Admin);

        var query = _orderRepository.GetQueryableWithDetails();

        if (!isAdmin)
        {
            query = query.Where(x => x.UserId == currentUserId);
        }

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
                    ErrorType.BadRequest);
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