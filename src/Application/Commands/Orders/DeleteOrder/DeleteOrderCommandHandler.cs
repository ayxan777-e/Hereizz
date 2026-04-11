using Application.Interfaces.Repositories;
using Application.Shared.Responses;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Commands.Orders.DeleteOrder;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, BaseResponse<bool>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteOrderCommandHandler(
        IOrderRepository orderRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _orderRepository = orderRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<BaseResponse<bool>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user is null)
        {
            return BaseResponse<bool>.Fail(
                "Unauthorized",
                new List<string> { "User context is missing." },
                ErrorType.Unauthorized);
        }

        var currentUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(currentUserId))
        {
            return BaseResponse<bool>.Fail(
                "Unauthorized",
                new List<string> { "User ID not found in token." },
                ErrorType.Unauthorized);
        }

        var isAdmin = user.IsInRole(Roles.Admin);

        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

        if (order is null)
        {
            return BaseResponse<bool>.Fail(
                "Order not found",
                new List<string> { "Order with given id does not exist." },
                ErrorType.NotFound);
        }

        if (!isAdmin && order.UserId != currentUserId)
        {
            return BaseResponse<bool>.Fail(
                "Forbidden",
                new List<string> { "You are not allowed to delete this order." },
                ErrorType.Forbidden);
        }

        _orderRepository.Delete(order);

        await _orderRepository.SaveChangesAsync(cancellationToken);

        return BaseResponse<bool>.Ok(true, "Order deleted successfully");
    }
}