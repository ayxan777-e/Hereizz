using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class NotificationRepository : GenericRepository<Notification, int>, INotificationRepository
{
    private readonly HereizzzDbContext _context;

    public NotificationRepository(HereizzzDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Notification>> GetByUserIdAsync(string userId, CancellationToken ct = default)
    {
        return await _context.Notifications
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<int> GetUnreadCountAsync(string userId, CancellationToken ct = default)
    {
        return await _context.Notifications
            .CountAsync(x => x.UserId == userId && !x.IsRead, ct);
    }

    public async Task<Notification?> GetByIdAndUserIdAsync(int id, string userId, CancellationToken ct = default)
    {
        return await _context.Notifications
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, ct);
    }

    public async Task MarkAllAsReadAsync(string userId, CancellationToken ct = default)
    {
        var notifications = await _context.Notifications
            .Where(x => x.UserId == userId && !x.IsRead)
            .ToListAsync(ct);

        foreach (var notification in notifications)
        {
            notification.IsRead = true;
        }
    }
}