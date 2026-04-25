using Application.DTOs.Payments;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Shared.Responses;
using MediatR;

namespace Application.Queries.Payments.GetMyPayments;

public class GetMyPaymentsQueryHandler
    : IRequestHandler<GetMyPaymentsQuery, BaseResponse<List<PaymentResponse>>>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetMyPaymentsQueryHandler(
        IPaymentRepository paymentRepository,
        ICurrentUserService currentUserService)
    {
        _paymentRepository = paymentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponse<List<PaymentResponse>>> Handle(
        GetMyPaymentsQuery request,
        CancellationToken ct)
    {
        if (!_currentUserService.IsAuthenticated || string.IsNullOrWhiteSpace(_currentUserService.UserId))
        {
            return BaseResponse<List<PaymentResponse>>.Fail(
                "Unauthorized",
                new List<string> { "User is not authenticated." },
                ErrorType.Unauthorized);
        }

        var payments = await _paymentRepository.GetByUserIdAsync(_currentUserService.UserId, ct);

        var response = payments.Select(x => new PaymentResponse
        {
            Id = x.Id,
            OrderId = x.OrderId,
            Amount = x.Amount,
            Method = x.Method,
            Status = x.Status,
            CreatedAt = x.CreatedAt,
            PaidAt = x.PaidAt,
            TransactionId = x.TransactionId
        }).ToList();

        return BaseResponse<List<PaymentResponse>>.Ok(response, "Payments fetched successfully");
    }
}