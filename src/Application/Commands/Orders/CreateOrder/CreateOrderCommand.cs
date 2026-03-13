using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.Orders.CreateOrder;

public class CreateOrderCommand : IRequest<BaseResponse<int>>
{
    public int ProductId { get; set; }

    public int ShippingOptionId { get; set; }
}