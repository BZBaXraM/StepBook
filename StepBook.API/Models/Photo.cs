using System.ComponentModel.DataAnnotations.Schema;

namespace StepBook.API.Models;

[Table("Photos")]
public class Photo
{
    public int Id { get; set; }
    public string Url { get; set; } = null!;
    public bool IsMain { get; set; }
    public string? PublicId { get; set; } 
    public User User { get; set; } = null!;
    public int UserId { get; set; }
}