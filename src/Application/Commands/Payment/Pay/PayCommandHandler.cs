using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Shared.Responses;
using Domain.Enums;
using MediatR;

namespace Application.Commands.Payments.Pay;

public class PayCommandHandler : IRequestHandler<PayCommand, BaseResponse>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly ICurrentUserService _currentUserService;

    public PayCommandHandler(
        IPaymentRepository paymentRepository,
        ICurrentUserService currentUserService)
    {
        _paymentRepository = paymentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponse> Handle(PayCommand request, CancellationToken ct)
    {
        var payment = await _paymentRepository.GetByIdAsync(request.PaymentId, ct);

        if (payment is null)
        {
            return BaseResponse.Fail(
                "Payment not found",
                new List<string> { "Payment not found." },
                ErrorType.NotFound);
        }

        if (payment.UserId != _currentUserService.UserId)
        {
            return BaseResponse.Fail(
                "Forbidden",
                new List<string> { "You are not allowed to pay this payment." },
                ErrorType.Forbidden);
        }

        if (payment.Status == PaymentStatus.Paid)
        {
            return BaseResponse.Fail(
                "Payment already completed",
                new List<string> { "This payment has already been paid." },
                ErrorType.BadRequest);
        }

        payment.Status = PaymentStatus.Paid;
        payment.PaidAt = DateTime.UtcNow;
        payment.TransactionId = Guid.NewGuid().ToString();

        _paymentRepository.Update(payment);
        await _paymentRepository.SaveChangesAsync(ct);

        return BaseResponse.Ok("Payment successful");
    }
}