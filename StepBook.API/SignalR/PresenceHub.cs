using Microsoft.AspNetCore.SignalR;

namespace StepBook.API.SignalR;

[Authorize]
public class PresenceHub(PresenceTracker tracker) : Hub
{
    public override async Task OnConnectedAsync()
    {
        if (Context.User is null)
            throw new HubException("User is null. Cannot get current user claim.");

        await tracker.UserConnected(Context.User.GetUsername()!, Context.ConnectionId);

        await Clients.Others.SendAsync("UserIsOnline", await tracker.GetOnlineUsers());
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (Context.User is null)
            throw new HubException("User is null. Cannot get current user claim.");

        await tracker.UserDisconnected(Context.User.GetUsername()!, Context.ConnectionId);

        await Clients.Others.SendAsync("UserIsOffline", Context.User?.GetUsername());

        await Clients.Others.SendAsync("UserIsOnline", await tracker.GetOnlineUsers());

        await base.OnDisconnectedAsync(exception);
    }
}