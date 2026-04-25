using Domain.Entities.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Payment : BaseEntity<int>
{
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;

    public decimal Amount { get; set; }

    public PaymentMethod Method { get; set; } = PaymentMethod.Card;

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? PaidAt { get; set; }

    public string? TransactionId { get; set; }

    public string? FailureReason { get; set; }
}