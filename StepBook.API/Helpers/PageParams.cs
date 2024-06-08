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
    public int PageNumber { get; set; } = 1;

    private int _pageSize = 10;

    /// <summary>
    /// The page size
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public string CurrentUsername { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    
    public int MinAge { get; set; } = 18;
    public int MaxAge { get; set; } = 77;
}