namespace StepBook.API.DTOs;

public class RefreshTokenDto
{
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}