using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.Orders.Checkout;

public class CheckoutCommand : IRequest<BaseResponse<int>>
{
}