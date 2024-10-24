namespace StepBook.API.Helpers;

/// <summary>
/// Pagination header class
/// </summary>
/// <param name="currentPage"></param>
/// <param name="itemsPerPage"></param>
/// <param name="totalItems"></param>
/// <param name="totalPages"></param>
public class PaginationHeader(int currentPage, int itemsPerPage, int totalItems, int totalPages)
{
    public int CurrentPage { get; set; } = currentPage;
    public int ItemsPerPage { get; set; } = itemsPerPage;
    public int TotalItems { get; set; } = totalItems;
    public int TotalPages { get; set; } = totalPages;
}