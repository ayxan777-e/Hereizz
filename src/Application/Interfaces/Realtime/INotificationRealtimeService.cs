namespace Application.Interfaces.Realtime;

public interface INotificationRealtimeService
{
    Task SendNotificationAsync(
        string userId,
        string title,
        string message,
        CancellationToken cancellationToken = default);
}