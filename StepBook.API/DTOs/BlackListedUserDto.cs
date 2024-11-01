namespace StepBook.API.DTOs;

/// <summary>
/// Data transfer object for blacklisted users.
/// </summary>
public class BlackListedUserDto
{
    /// <summary>
    /// The name of the user.
    /// </summary>
    public required string UserName { get; set; }

    /// <summary>
    /// The known as name of the user.
    /// </summary>
    public required string KnownAs { get; set; }
}