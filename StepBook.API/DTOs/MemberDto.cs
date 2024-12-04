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
    /// The photo URL
    /// </summary>
    public string? PhotoUrl { get; init; }

    /// <summary>
    /// The Age
    /// </summary>
    public int Age { get; init; }

    /// <summary>
    /// The first name
    /// </summary>
    public string FirstName { get; init; } = null!;
    /// <summary>
    /// The last name
    /// </summary>
    public string LastName { get; init; } = null!;

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
    public string? Gender { get; init; }

    /// <summary>
    /// The Introduction
    /// </summary>
    public string? Introduction { get; init; }

    /// <summary>
    /// Looking for
    /// </summary>
    public string? LookingFor { get; init; }

    /// <summary>
    /// Interests
    /// </summary>
    public string? Interests { get; init; }

    /// <summary>
    /// City
    /// </summary>
    public string? City { get; init; }

    /// <summary>
    /// Country
    /// </summary>
    public string? Country { get; init; }

    /// <summary>
    /// The photos of the user
    /// </summary>
    public ICollection<PhotoDto?> Photos { get; init; } = [];
}