using System.Collections.Concurrent;

namespace StepBook.API.Hubs;

public class PresenceTracker
{
    private static readonly ConcurrentDictionary<string, List<string>> OnlineUsers = new();

    public Task<bool> UserConnected(string username, string connectionId)
    {
        var isOnline = false;
        OnlineUsers.AddOrUpdate(username,
            [connectionId],
            (key, oldValue) =>
            {
                oldValue.Add(connectionId);
                return oldValue;
            });

        if (OnlineUsers[username].Count == 1)
        {
            isOnline = true;
        }

        return Task.FromResult(isOnline);
    }

    public Task<bool> UserDisconnected(string username, string connectionId)
    {
        var isOffline = false;
        if (OnlineUsers.TryGetValue(username, out var connections))
        {
            connections.Remove(connectionId);
            if (connections.Count == 0)
            {
                OnlineUsers.TryRemove(username, out _);
                isOffline = true;
            }
        }

        return Task.FromResult(isOffline);
    }

    public Task<string[]> GetOnlineUsers()
    {
        var onlineUsers = OnlineUsers.Keys.OrderBy(k => k).ToArray();
        return Task.FromResult(onlineUsers);
    }

    public Task<List<string>> GetConnectionsForUser(string username)
    {
        OnlineUsers.TryGetValue(username, out var connections);
        return Task.FromResult(connections ?? []);
    }
}