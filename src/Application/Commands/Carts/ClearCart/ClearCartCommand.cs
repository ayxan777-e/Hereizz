using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.Cart.ClearCart;

public class ClearCartCommand : IRequest<BaseResponse>
{
}