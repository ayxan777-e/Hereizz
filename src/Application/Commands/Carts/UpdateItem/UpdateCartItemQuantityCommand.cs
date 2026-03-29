using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.Cart.UpdateItemQuantity;

public class UpdateCartItemQuantityCommand : IRequest<BaseResponse>
{
    public int ItemId { get; set; }
    public int Quantity { get; set; }
}