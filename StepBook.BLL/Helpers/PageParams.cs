namespace StepBook.API.Helpers;

/// <summary>
/// The page parameters
/// </summary>
public class PageParams : PaginationParams
{
    /// <summary>
    /// The current username
    /// </summary>
    public string CurrentUsername { get; set; } = string.Empty;

    /// <summary>
    /// The gender
    /// </summary>
    public string Gender { get; set; } = string.Empty;

    /// <summary>
    /// The minimum age
    /// </summary>
    public int MinAge { get; set; } = 18;

    /// <summary>
    /// The maximum age
    /// </summary>
    public int MaxAge { get; set; } = 77;

    /// <summary>
    /// The order by 
    /// </summary>
    public string OrderBy { get; set; } = "lastActive";
}