using System.ComponentModel.DataAnnotations.Schema;

namespace StepBook.API.Models;

/// <summary>
/// The photo class
/// </summary>
[Table("Photos")]
public class Photo
{
    public int Id { get; set; }
    public string Url { get; set; } = null!;
    public bool IsMain { get; set; }
    public string? PublicId { get; set; }
    public User User { get; set; }
    public int UserId { get; set; }
}