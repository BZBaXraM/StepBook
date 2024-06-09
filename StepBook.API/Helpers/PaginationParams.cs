namespace StepBook.API.Helpers;

/// <summary>
/// The pagination parameters
/// </summary>
public class PaginationParams
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
}