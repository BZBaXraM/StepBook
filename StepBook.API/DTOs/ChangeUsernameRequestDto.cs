namespace StepBook.API.DTOs;

/// <summary>
/// Change username request DTO
/// </summary>
public class ChangeUsernameRequestDto
{
    /// <summary>
    /// New username
    /// </summary>
    public required string NewUsername { get; set; }
}