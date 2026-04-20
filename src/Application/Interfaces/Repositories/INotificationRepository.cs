using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface INotificationRepository : IGenericRepository<Notification, int>
{
    Task<List<Notification>> GetByUserIdAsync(string userId, CancellationToken ct = default);
    Task<int> GetUnreadCountAsync(string userId, CancellationToken ct = default);
    Task<Notification?> GetByIdAndUserIdAsync(int id, string userId, CancellationToken ct = default);
    Task MarkAllAsReadAsync(string userId, CancellationToken ct = default);
}