using Domain.Entities.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Notification : BaseEntity<int>
{
    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;

    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;

    public NotificationType Type { get; set; }

    public bool IsRead { get; set; } = false;
}