namespace StepBook.API.DTOs;

/// <summary>
/// DTO for reports.
/// </summary>
public class ReportDto
{
    /// <summary>
    /// The username of the reporter.
    /// </summary>
    public required string ReporterUsername { get; set; }

    /// <summary>
    /// The username of the reported user.
    /// </summary>
    public required string ReportedUsername { get; set; }

    /// <summary>
    /// The reason for the report.
    /// </summary>
    public required string Reason { get; set; }

    /// <summary>
    /// The date and time the report was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}