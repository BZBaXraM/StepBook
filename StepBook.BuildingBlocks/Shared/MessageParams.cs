namespace StepBook.BuildingBlocks.Shared;

/// <summary>
/// The message parameters 
/// </summary>
public class MessageParams : PaginationParams
{
    /// <summary>
    /// The username
    /// </summary>
    public string Username { get; set; } = string.Empty;
    /// <summary>
    /// The container
    /// </summary>
    public string Container { get; set; } = "Unread";
}