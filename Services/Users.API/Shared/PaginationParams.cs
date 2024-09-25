namespace Users.API.Shared;

public class PaginationParams
{
    private const int MaxPageSize = 50;

    /// <summary>
    /// The page number
    /// </summary>
    public int PageNumber { get; set; } = 1;

    private int _pageSize = 10;

    /// <summary>
    /// The page sizez
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
}