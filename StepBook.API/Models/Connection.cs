namespace StepBook.API.Models;

public class Connection
{
    public Connection(string connectionId, string username)
    {
        ConnectionId = connectionId;
        Username = username;
    }

    public Connection()
    {
    }

    public string ConnectionId { get; set; }
    public string Username { get; set; }
}