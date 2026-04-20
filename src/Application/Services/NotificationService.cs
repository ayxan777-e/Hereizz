using Application.DTOs.Notifications;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Shared.Responses;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ICurrentUserService _currentUserService;

    public NotificationService(
        INotificationRepository notificationRepository,
        ICurrentUserService currentUserService)
    {
        _notificationRepository = notificationRepository;
        _currentUserService = currentUserService;
    }

    public async Task CreateAsync(string userId, string title, string message, NotificationType type, CancellationToken ct = default)
    {
        var notification = new Notification
        {
            UserId = userId,
            Title = title,
            Message = message,
            Type = type,
            IsRead = false
        };

        await _notificationRepository.AddAsync(notification, ct);
        await _notificationRepository.SaveChangesAsync(ct);
    }

    public async Task<BaseResponse<List<NotificationResponse>>> GetMyNotificationsAsync(CancellationToken ct)
    {
        if (!_currentUserService.IsAuthenticated || string.IsNullOrWhiteSpace(_currentUserService.UserId))
        {
            return BaseResponse<List<NotificationResponse>>.Fail(
                "Unauthorized",
                new List<string> { "User is not authenticated." },
                ErrorType.Unauthorized);
        }

        var notifications = await _notificationRepository.GetByUserIdAsync(_currentUserService.UserId, ct);

        var response = notifications.Select(x => new NotificationResponse
        {
            Id = x.Id,
            Title = x.Title,
            Message = x.Message,
            Type = x.Type.ToString(),
            IsRead = x.IsRead,
            CreatedAt = x.CreatedAt
        }).ToList();

        return BaseResponse<List<NotificationResponse>>.Ok(response, "Notifications fetched successfully");
    }

    public async Task<BaseResponse<int>> GetUnreadCountAsync(CancellationToken ct)
    {
        if (!_currentUserService.IsAuthenticated || string.IsNullOrWhiteSpace(_currentUserService.UserId))
        {
            return BaseResponse<int>.Fail(
                "Unauthorized",
                new List<string> { "User is not authenticated." },
                ErrorType.Unauthorized);
        }

        var count = await _notificationRepository.GetUnreadCountAsync(_currentUserService.UserId, ct);

        return BaseResponse<int>.Ok(count, "Unread notification count fetched successfully");
    }

    public async Task<BaseResponse> MarkAsReadAsync(int notificationId, CancellationToken ct)
    {
        if (!_currentUserService.IsAuthenticated || string.IsNullOrWhiteSpace(_currentUserService.UserId))
        {
            return BaseResponse.Fail(
                "Unauthorized",
                new List<string> { "User is not authenticated." },
                ErrorType.Unauthorized);
        }

        var notification = await _notificationRepository.GetByIdAndUserIdAsync(notificationId, _currentUserService.UserId, ct);

        if (notification is null)
        {
            return BaseResponse.Fail(
                "Notification not found",
                new List<string> { "Notification with given id does not exist." },
                ErrorType.NotFound);
        }

        if (!notification.IsRead)
        {
            notification.IsRead = true;
            _notificationRepository.Update(notification);
            await _notificationRepository.SaveChangesAsync(ct);
        }

        return BaseResponse.Ok("Notification marked as read");
    }

    public async Task<BaseResponse> MarkAllAsReadAsync(CancellationToken ct)
    {
        if (!_currentUserService.IsAuthenticated || string.IsNullOrWhiteSpace(_currentUserService.UserId))
        {
            return BaseResponse.Fail(
                "Unauthorized",
                new List<string> { "User is not authenticated." },
                ErrorType.Unauthorized);
        }

        await _notificationRepository.MarkAllAsReadAsync(_currentUserService.UserId, ct);
        await _notificationRepository.SaveChangesAsync(ct);

        return BaseResponse.Ok("All notifications marked as read");
    }
}