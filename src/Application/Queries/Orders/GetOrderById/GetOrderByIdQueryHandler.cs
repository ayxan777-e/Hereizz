using Application.DTOs.Orders;
using Application.Interfaces.Repositories;
using Application.Shared.Responses;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Queries.Orders.GetOrderById;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, BaseResponse<OrderDetailsDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetOrderByIdQueryHandler(
        IOrderRepository orderRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _orderRepository = orderRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<BaseResponse<OrderDetailsDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user is null)
        {
            return BaseResponse<OrderDetailsDto>.Fail(
                "Unauthorized",
                new List<string> { "User context is missing." },
                ErrorType.Unauthorized);
        }

        var currentUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(currentUserId))
        {
            return BaseResponse<OrderDetailsDto>.Fail(
                "Unauthorized",
                new List<string> { "User ID not found in token." },
                ErrorType.Unauthorized);
        }

        var isAdmin = user.IsInRole(Roles.Admin);

        var order = await _orderRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);

        if (order is null)
        {
            return BaseResponse<OrderDetailsDto>.Fail(
                "Order not found",
                new List<string> { "Order with given id does not exist." },
                ErrorType.NotFound);
        }

        if (!isAdmin && order.UserId != currentUserId)
        {
            return BaseResponse<OrderDetailsDto>.Fail(
                "Forbidden",
                new List<string> { "You are not allowed to access this order." },
                ErrorType.Forbidden);
        }

        var dto = new OrderDetailsDto
        {
            Id = order.Id,
            UserId = order.UserId,
            TotalPrice = order.TotalPrice,
            Status = order.Status.ToString(),
            CreatedAt = order.CreatedAt,
            Items = order.Items.Select(x => new OrderItemDetailsDto
            {
                Id = x.Id,
                ProductId = x.ProductId,
                ProductTitle = x.ProductTitle,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                ShippingOptionId = x.ShippingOptionId,
                ShippingOptionName = x.ShippingOptionName,
                ShippingCost = x.ShippingCost,
                CustomsFee = x.CustomsFee,
                WarehouseFee = x.WarehouseFee,
                LocalDeliveryFee = x.LocalDeliveryFee,
                FinalPrice = x.FinalPrice,
                TransportType = x.TransportType,
                EstimatedMinDays = x.EstimatedMinDays,
                EstimatedMaxDays = x.EstimatedMaxDays
            }).ToList()
        };

        return BaseResponse<OrderDetailsDto>.Ok(dto, "Order fetched successfully");
    }
}