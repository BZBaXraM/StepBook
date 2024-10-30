namespace StepBook.API.DTOs;

/// <summary>
/// Data transfer object for blacklisted users.
/// </summary>
public class BlackListedUserDto
{
    public required string UserName { get; set; }
    public required string KnownAs { get; set; }
}