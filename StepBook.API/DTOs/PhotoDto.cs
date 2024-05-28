namespace StepBook.API.DTOs;

/// <summary>
/// The photo DTO
/// </summary>
public class  PhotoDto
{
    /// <summary>
    /// The id
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The URL
    /// </summary>
    public string Url { get; init; } = null!;

    /// <summary>
    /// The description
    /// </summary>
    public bool IsMain { get; init; }
}