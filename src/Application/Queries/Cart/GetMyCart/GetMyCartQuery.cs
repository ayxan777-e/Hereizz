using Application.DTOs.Cart;
using Application.Shared.Responses;
using MediatR;

namespace Application.Queries.Cart.GetMyCart;

public class GetMyCartQuery: IRequest<BaseResponse<CartDto>>
{
}
