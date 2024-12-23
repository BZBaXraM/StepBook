namespace StepBook.DTO.DTOs;

/// <summary>
/// Represents the data transfer object for confirming email code.
/// </summary>
public class ConfirmEmailCodeDto
{   /// <summary>
    /// The code to confirm the email.
    /// </summary>
    public required string Code { get; set; }
}