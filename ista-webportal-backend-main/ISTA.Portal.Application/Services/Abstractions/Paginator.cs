using Microsoft.EntityFrameworkCore;

namespace ISTA.Portal.Application.Services.Abstractions;

public static class Paginator
{
    public const int PAGE_SIZE = 10;

    public static async Task<ResponseWithPagination<T>> AddPagination<T>(this IQueryable<T> query, int pageNum, CancellationToken ct) where T : class
    {
        var totalCount = await query.CountAsync(ct);

        var data = await query.Skip((pageNum - 1) * PAGE_SIZE).Take(PAGE_SIZE).ToListAsync(ct);

        return new ResponseWithPagination<T>
        {
            TotalCount = totalCount,
            Filtered = data.Count,
            CurrentPage = pageNum,
            Data = data
        };
    }
}