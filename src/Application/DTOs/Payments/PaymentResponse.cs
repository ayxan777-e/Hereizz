using Domain.Enums;

namespace Application.DTOs.Payments;

public class PaymentResponse
{
    public int Id { get; set; }
    public int? OrderId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; }
    public PaymentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? PaidAt { get; set; }
    public string? TransactionId { get; set; }
}