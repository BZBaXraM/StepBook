namespace StepBook.API.DTOs;

/// <summary>
/// Represents a data transfer object for a like.
/// </summary>
public class LikeDto
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public int Age { get; set; }
    public string KnownAs { get; set; } = null!;
    public string? PhotoUrl { get; set; }
    public string City { get; set; } = null!;
}