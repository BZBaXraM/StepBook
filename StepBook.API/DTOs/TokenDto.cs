namespace StepBook.API.DTOs;

public class TokenDto
{
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public DateTime RefreshTokenExpireTime { get; set; }
}