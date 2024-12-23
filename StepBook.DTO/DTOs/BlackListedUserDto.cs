namespace StepBook.DTO.DTOs;

public class BlackListedUserDto
{
    /// <summary>
    /// The name of the user.
    /// </summary>
    public required string UserName { get; set; }

    /// <summary>
    /// The known as name of the user.
    /// </summary>
    public required string FirstName { get; set; }
}