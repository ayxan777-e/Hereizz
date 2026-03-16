using Application.Interfaces.Repositories;
using Application.Shared.Responses;
using Domain.Enums;
using MediatR;

namespace Application.Commands.Orders.UpdateOrderStatus;

public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, BaseResponse<bool>>
{
    private readonly IOrderRepository _orderRepository;

    public UpdateOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<BaseResponse<bool>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
       
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

        if (order is null)
        {
            return BaseResponse<bool>.Fail(
                "Order not found",
                new List<string> { "Order with given id does not exist" }
            );
        }

        order.Status = request.Status;

        await _orderRepository.SaveChangesAsync(cancellationToken);

        return BaseResponse<bool>.Ok(true, "Order status updated");
    }
}