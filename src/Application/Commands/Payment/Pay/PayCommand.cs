using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.Payments.Pay;

public class PayCommand : IRequest<BaseResponse>
{
    public int PaymentId { get; set; }
}