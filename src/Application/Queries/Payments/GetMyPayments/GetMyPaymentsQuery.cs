using Application.DTOs.Payments;
using Application.Shared.Responses;
using MediatR;

namespace Application.Queries.Payments.GetMyPayments;

public class GetMyPaymentsQuery : IRequest<BaseResponse<List<PaymentResponse>>>
{
}