namespace StepBook.API.Helpers;

/// <summary>
/// The page parameters
/// </summary>
public class PageParams
{
    private const int MaxPageSize = 50;

    /// <summary>
    /// The page number
    /// </summary>
    public static int PageNumber => 1;

    private int _pageSize = 10;

    /// <summary>
    /// The page size
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    /// <summary>
    /// The current username
    /// </summary>
    public string? CurrentUsername { get; set; } = string.Empty;
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