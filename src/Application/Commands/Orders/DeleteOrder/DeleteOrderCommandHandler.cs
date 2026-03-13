using Application.Interfaces.Repositories;
using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.Orders.DeleteOrder;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, BaseResponse<bool>>
{
    private readonly IOrderRepository _orderRepository;

    public DeleteOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<BaseResponse<bool>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

        if (order is null)
        {
            return BaseResponse<bool>.Fail(
                "Order not found",
                new List<string> { "Order with given id does not exist" }
            );
        }

        _orderRepository.Delete(order);

        await _orderRepository.SaveChangesAsync(cancellationToken);

        return BaseResponse<bool>.Ok(true, "Order deleted successfully");
    }
}