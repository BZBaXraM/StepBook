namespace StepBook.API.DTOs;

/// <summary>
/// Google sign in DTO
/// </summary>
public class GoogleSignInDto
{
    /// <summary>
    /// Id token
    /// </summary>
    public required string IdToken { get; init; } = default!;
}