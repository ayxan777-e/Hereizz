using Application.DTOs.Payments;
using Application.Shared.Responses;
using MediatR;

namespace Application.Queries.Payments.GetAllPayments;

public class GetAllPaymentsQuery : IRequest<BaseResponse<List<PaymentResponse>>>
{
}