using Application.DTOs.Payments;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Shared.Responses;
using MediatR;

namespace Application.Queries.Payments.GetAllPayments;

public class GetAllPaymentsQueryHandler
    : IRequestHandler<GetAllPaymentsQuery, BaseResponse<List<PaymentResponse>>>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetAllPaymentsQueryHandler(
        IPaymentRepository paymentRepository,
        ICurrentUserService currentUserService)
    {
        _paymentRepository = paymentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponse<List<PaymentResponse>>> Handle(
        GetAllPaymentsQuery request,
        CancellationToken ct)
    {
        if (!_currentUserService.IsInRole("Admin"))
        {
            return BaseResponse<List<PaymentResponse>>.Fail(
                "Forbidden",
                new List<string> { "Only admins can access all payments." },
                ErrorType.Forbidden);
        }

        var payments = await _paymentRepository.GetAllAsync(ct);

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

        return BaseResponse<List<PaymentResponse>>.Ok(response, "All payments fetched");
    }
}