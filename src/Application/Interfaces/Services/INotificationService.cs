using Application.DTOs.Notifications;
using Application.Shared.Responses;
using Domain.Enums;

namespace Application.Interfaces.Services;

public interface INotificationService
{
    Task CreateAsync(string userId, string title, string message, NotificationType type, CancellationToken ct = default);
    Task<BaseResponse<List<NotificationResponse>>> GetMyNotificationsAsync(CancellationToken ct);
    Task<BaseResponse<int>> GetUnreadCountAsync(CancellationToken ct);
    Task<BaseResponse> MarkAsReadAsync(int notificationId, CancellationToken ct);
    Task<BaseResponse> MarkAllAsReadAsync(CancellationToken ct);
}