using System.Text.Json;

namespace StepBook.API.Extensions;

/// <summary>
/// The http extensions
/// </summary>
public static class HttpExtensions
{
    /// <summary>
    /// Add pagination to the response
    /// </summary>
    /// <param name="response"></param>
    /// <param name="currentPage"></param>
    /// <param name="itemsPerPage"></param>
    /// <param name="totalItems"></param>
    /// <param name="totalPages"></param>
    public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems,
        int totalPages)
    {
        var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        response.Headers.Append("Pagination", JsonSerializer.Serialize(paginationHeader, options));
        response.Headers.Append("Access-Control-Expose-Headers", "Pagination");
    }
}