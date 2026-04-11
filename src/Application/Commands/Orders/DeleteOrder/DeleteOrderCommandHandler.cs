using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Shared.Responses;
using Domain.Constants;
using MediatR;

namespace Application.Commands.Orders.DeleteOrder;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, BaseResponse<bool>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteOrderCommandHandler(
        IOrderRepository orderRepository,
        ICurrentUserService currentUserService)
    {
        _orderRepository = orderRepository;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponse<bool>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated)
        {
            return BaseResponse<bool>.Fail(
                "Unauthorized",
                new List<string> { "User is not authenticated." },
                ErrorType.Unauthorized);
        }

        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrWhiteSpace(currentUserId))
        {
            return BaseResponse<bool>.Fail(
                "Unauthorized",
                new List<string> { "User ID not found in token." },
                ErrorType.Unauthorized);
        }

        var isAdmin = _currentUserService.IsInRole(Roles.Admin);

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