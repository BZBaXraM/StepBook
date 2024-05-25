namespace StepBook.API.DTOs;

/// <summary>
/// The member DTO
/// </summary>
public class MemberDto
{
    /// <summary>
    /// The id
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The username
    /// </summary>
    public string Username { get; init; } = null!;

    /// <summary>
    /// The Age
    /// </summary>
    public int Age { get; init; }

    /// <summary>
    /// The known as
    /// </summary>
    public string KnownAs { get; init; } = null!;

    /// <summary>
    /// Created
    /// </summary>
    public DateTime Created { get; init; }

    /// <summary>
    /// The photo URL
    /// </summary>
    public DateTime LastActive { get; init; }

    /// <summary>
    /// Gender
    /// </summary>
    public string Gender { get; init; } = null!;

    /// <summary>
    /// The Introduction
    /// </summary>
    public string Introduction { get; init; } = null!;

    /// <summary>
    /// Looking for
    /// </summary>
    public string LookingFor { get; init; } = null!;

    /// <summary>
    /// Interests
    /// </summary>
    public string Interests { get; init; } = null!;

    /// <summary>
    /// City
    /// </summary>
    public string City { get; init; } = null!;

    /// <summary>
    /// Country
    /// </summary>
    public string Country { get; init; } = null!;

    /// <summary>
    /// The photo URL
    /// </summary>
    public string? PhotoUrl { get; init; } // Add this property


    /// <summary>
    /// The photos of the user
    /// </summary>
    public ICollection<PhotoDto>? Photos { get; init; }
}