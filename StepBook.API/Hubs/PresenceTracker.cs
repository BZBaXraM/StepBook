namespace StepBook.API.Hubs;

public class PresenceTracker
{
    private static readonly Dictionary<string, List<string>> OnlineUsers = new();

    public Task<bool> UserConnected(string username, string connectionId)
    {
        var isOnline = false;
        lock (OnlineUsers)
        {
            if (OnlineUsers.TryGetValue(username, out var user))
            {
                user.Add(connectionId);
            }
            else
            {
                OnlineUsers.Add(username, [connectionId]);
                isOnline = true;
            }
        }

        return Task.FromResult(isOnline);
    }

    public Task<bool> UserDisconnected(string username, string connectionId)
    {
        var isOffline = false;
        lock (OnlineUsers)
        {
            if (!OnlineUsers.TryGetValue(username, out var user))
            {
                return Task.FromResult(isOffline);
            }

            user.Remove(connectionId);
            if (user.Count == 0)
            {
                OnlineUsers.Remove(username);
                isOffline = true;
            }
        }

        return Task.FromResult(isOffline);
    }

    public Task<string[]> GetOnlineUsers()
    {
        string[] onlineUsers;
        lock (OnlineUsers)
        {
            onlineUsers = OnlineUsers.OrderBy(k => k.Key)
                .Select(k => k.Key)
                .ToArray();
        }

        return Task.FromResult(onlineUsers);
    }

    public Task<List<string>> GetConnectionsForUser(string username)
    {
        List<string>? connections;
        lock (OnlineUsers)
        {
            connections = OnlineUsers.GetValueOrDefault(username);
        }

        return Task.FromResult(connections)!;
    }
}