using Domain.Entities.Common;

namespace Domain.Entities;

public class Cart: BaseEntity<int>
{
    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;

    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}