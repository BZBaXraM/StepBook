using Microsoft.AspNetCore.SignalR;

namespace StepBook.API.Hubs;

/// <summary>
/// SignalR hub for presence tracker.
/// </summary>
/// <param name="tracker"></param>
[Authorize]
public class PresenceHub(PresenceTracker tracker) : Hub
{
    /// <inheritdoc />
    public override async Task OnConnectedAsync()
    {
        if (Context.User is null)
            throw new HubException("User is null. Cannot get current user claim.");

        var isOnline = await tracker.UserConnected(Context.User.GetUsername()!, Context.ConnectionId);
        if (isOnline)
            await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUsername());

        await Clients.Caller.SendAsync("UserIsOnline", await tracker.GetOnlineUsers());
    }

    /// <inheritdoc />
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (Context.User is null)
            throw new HubException("User is null. Cannot get current user claim.");

        var isOffline = await tracker.UserDisconnected(Context.User.GetUsername()!, Context.ConnectionId);
        if (isOffline)
            await Clients.Others.SendAsync("UserIsOffline", Context.User?.GetUsername());

        await base.OnDisconnectedAsync(exception);
    }
}