namespace StepBook.API.Hubs;

[Authorize]
public class PresenceHub(PresenceTracker tracker) : Hub
{
    public override async Task OnConnectedAsync()
    {
        await tracker.UserConnected(Context.User.GetUsername(), Context.ConnectionId);
        await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUsername());

        await Clients.All.SendAsync("GetOnlineUsers", tracker.GetOnlineUsers());
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await tracker.UserDisconnected(Context.User!.GetUsername()!, Context.ConnectionId);
        await Clients.Others.SendAsync("UserIsOffline", Context.User!.GetUsername()!);

        await Clients.All.SendAsync("GetOnlineUsers", tracker.GetOnlineUsers());

        await base.OnDisconnectedAsync(exception);
    }
}