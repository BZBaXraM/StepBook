namespace Messages.API.Models;

/// <summary>
/// Represents a connection in the application.
/// </summary>
public class Connection
{
    /// <summary>
    /// Connection id
    /// </summary>
    public required string ConnectionId { get; set; }
    /// <summary>
    /// Username
    /// </summary>
    public required string Username { get; set; }
}