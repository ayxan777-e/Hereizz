using API.Controllers.Common;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NotificationsController : BaseApiController
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyNotifications(CancellationToken ct)
    {
        var response = await _notificationService.GetMyNotificationsAsync(ct);
        return HandleResponse(response);
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount(CancellationToken ct)
    {
        var response = await _notificationService.GetUnreadCountAsync(ct);
        return HandleResponse(response);
    }

    [HttpPatch("{id}/read")]
    public async Task<IActionResult> MarkAsRead(int id, CancellationToken ct)
    {
        var response = await _notificationService.MarkAsReadAsync(id, ct);
        return HandleResponse(response);
    }

    [HttpPatch("read-all")]
    public async Task<IActionResult> MarkAllAsRead(CancellationToken ct)
    {
        var response = await _notificationService.MarkAllAsReadAsync(ct);
        return HandleResponse(response);
    }
}