namespace StepBook.DTO.DTOs;

/// <summary>
/// The member update DTO
/// </summary>
public class MemberUpdateDto
{
    /// <summary>
    /// Introduction
    /// </summary>
    public string? Introduction { get; set; }

    /// <summary>
    /// Looking for
    /// </summary>
    public string? LookingFor { get; set; }

    /// <summary>
    /// Interests
    /// </summary> 
    public string? Interests { get; set; }

    /// <summary>
    /// City
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// Country
    /// </summary>
    public string? Country { get; set; }
}