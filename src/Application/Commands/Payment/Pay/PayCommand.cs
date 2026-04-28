using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.Payments.Pay;

public class PayCommand : IRequest<BaseResponse<int>>
{
    public int PaymentId { get; set; }
}