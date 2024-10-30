namespace StepBook.API.Models;

/// <summary>
/// The report model
/// </summary>
public class Report
{
    public int Id { get; set; }
    public int ReporterId { get; set; }
    public User Reporter { get; set; }
    public int ReportedId { get; set; }
    public User Reported { get; set; }
    public string Reason { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}