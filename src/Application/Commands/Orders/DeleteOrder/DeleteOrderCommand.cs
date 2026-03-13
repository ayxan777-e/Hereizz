using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.Orders.DeleteOrder;

public class DeleteOrderCommand : IRequest<BaseResponse<bool>>
{
    public int OrderId { get; set; }

    public DeleteOrderCommand(int orderId)
    {
        OrderId = orderId;
    }
}