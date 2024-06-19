namespace StepBook.API.Hubs;

[Authorize]
public class PresenceHub(PresenceTracker tracker) : Hub
{
    public override async Task OnConnectedAsync()
    {
        var isOnline = await tracker.UserConnected(Context.User.GetUsername(), Context.ConnectionId);
        if (isOnline)
        {
            await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUsername());
        }

        await Clients.Caller.SendAsync("GetOnlineUsers", tracker.GetOnlineUsers());
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var isOffline = await tracker.UserDisconnected(Context.User!.GetUsername()!, Context.ConnectionId);
        if (isOffline)
        {
            await Clients.Others.SendAsync("UserIsOffline", Context.User!.GetUsername()!);
        }

        await base.OnDisconnectedAsync(exception);
    }
}