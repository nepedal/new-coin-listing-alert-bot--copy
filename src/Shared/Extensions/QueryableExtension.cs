namespace Shared.Extensions;

public static class QueryableExtension
{
    public static IQueryable<T> WithPagination<T>(this IQueryable<T> query, PaginationParams paginationParams)
    {
        return query.Skip(paginationParams.Offset).Take(paginationParams.Limit);
    }
}
