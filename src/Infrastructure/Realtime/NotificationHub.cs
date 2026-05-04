using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Realtime;

public class NotificationHub : Hub
{
    public async Task JoinUserGroup(string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userId);
    }
}