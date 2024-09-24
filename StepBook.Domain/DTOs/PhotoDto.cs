namespace StepBook.Domain.DTOs;

/// <summary>
/// The photo DTO
/// </summary>
public class PhotoDto
{
    /// <summary>
    /// The id
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The URL
    /// </summary>
    public required string Url { get; init; } = null!;

    /// <summary>
    /// The description
    /// </summary>
    public bool IsMain { get; init; }

    /// <summary>
    /// Is the photo approved by the user or not
    /// </summary>
    public bool IsApproved { get; init; }
}