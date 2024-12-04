namespace StepBook.API.Models;

/// <summary>
/// The report model.
/// </summary>
public class Report
{
    /// <summary>
    /// The report id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The reporter id.
    /// </summary>
    public int ReporterId { get; set; }

    /// <summary>
    /// The reporter.
    /// </summary>
    public User Reporter { get; set; }

    /// <summary>
    /// The reported id.
    /// </summary>
    public int ReportedId { get; set; }

    /// <summary>
    /// The reported.
    /// </summary>
    public User Reported { get; set; }

    /// <summary>
    /// The reason for the report.
    /// </summary>
    public string Reason { get; set; }

    /// <summary>
    /// The date the report was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}