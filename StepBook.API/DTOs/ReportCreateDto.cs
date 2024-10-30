namespace StepBook.API.DTOs;

/// <summary>
/// Data transfer object for creating a report.
/// </summary>
public class ReportCreateDto
{
    /// <summary>
    /// Reason for the report.
    /// </summary>
    public required string Reason { get; set; }
}