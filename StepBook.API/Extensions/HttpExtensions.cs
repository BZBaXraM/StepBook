using System.Text.Json;
using StepBook.BLL.Helpers;
using StepBook.BuildingBlocks.Shared;

namespace StepBook.API.Extensions;

/// <summary>
/// The http extensions
/// </summary>
public static class HttpExtensions
{
    public static void AddPaginationHeader<T>(this HttpResponse response, PageList<T> data)
    {
        var paginationHeader = new PaginationHeader(data.CurrentPage, data.PageSize,
            data.TotalCount, data.TotalPages);

        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        response.Headers.Append("Pagination", JsonSerializer.Serialize(paginationHeader, jsonOptions));
        response.Headers.Append("Access-Control-Expose-Headers", "Pagination");
    }
}