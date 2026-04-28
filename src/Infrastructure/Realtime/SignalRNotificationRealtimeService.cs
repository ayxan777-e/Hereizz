using Application.Interfaces.Realtime;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Realtime;

public class SignalRNotificationRealtimeService : INotificationRealtimeService
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public SignalRNotificationRealtimeService(
        IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendNotificationAsync(
        string userId,
        string title,
        string message,
        CancellationToken cancellationToken = default)
    {
        await _hubContext.Clients
            .Group(userId)
            .SendAsync(
                "ReceiveNotification",
                new
                {
                    title,
                    message
                },
                cancellationToken);
    }
}