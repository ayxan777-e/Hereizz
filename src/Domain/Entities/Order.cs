using Domain.Entities.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Order : BaseEntity<int>
{
    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public decimal TotalPrice { get; set; }

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    public Payment? Payment { get; set; }
}