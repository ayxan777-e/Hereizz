using Application.Shared.Responses;
using Domain.Enums;
using MediatR;

namespace Application.Commands.Orders.UpdateOrderStatus;

public class UpdateOrderStatusCommand : IRequest<BaseResponse<bool>>
{
    public int OrderId { get; set; }

    public OrderStatus Status { get; set; }
}