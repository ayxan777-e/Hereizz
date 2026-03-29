using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.Cart.RemoveItem;

public class RemoveCartItemCommand : IRequest<BaseResponse>
{
    public int ItemId { get; set; }
}