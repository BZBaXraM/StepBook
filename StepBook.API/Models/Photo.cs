namespace StepBook.API.Models;

/// <summary>
/// The photo class
/// </summary>
[Table("Photos")]
public class Photo
{
    /// <summary>
    /// The unique identifier for the photo.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The url of the photo.
    /// </summary>
    public required string Url { get; set; }

    /// <summary>
    /// Main photo or not.
    /// </summary>
    public bool IsMain { get; set; }

    /// <summary>
    /// Public id of the photo.
    /// </summary>
    public string? PublicId { get; set; }

    /// <summary>
    /// Is approved or not.
    /// </summary>
    public bool IsApproved { get; set; } = false;

    /// <summary>
    /// User of the photo.
    /// </summary>
    public User User { get; set; } = default!;

    /// <summary>
    /// User id of the photo.
    /// </summary>
    public int UserId { get; set; }
}