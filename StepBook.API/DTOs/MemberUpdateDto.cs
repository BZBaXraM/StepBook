namespace StepBook.API.DTOs;

/// <summary>
/// The member update DTO
/// </summary>
public class MemberUpdateDto
{
    /// <summary>
    /// Introduction
    /// </summary>
    public string Introduction { get; set; } = null!;

    /// <summary>
    /// Looking for
    /// </summary>
    public string LookingFor { get; set; } = null!;

    /// <summary>
    /// Interests
    /// </summary> 
    public string Interests { get; set; } = null!;

    /// <summary>
    /// City
    /// </summary>
    public string City { get; set; } = null!;

    /// <summary>
    /// Country
    /// </summary>
    public string Country { get; set; } = null!;
}