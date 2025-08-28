namespace dotnet_pagination_basics.Services.DTOs;

public record KeysetPaginatedResults<T>(
    IEnumerable<T> Items,
    int PageSize,
    int? LastId,
    bool HasNextPage,
    int? TotalCount = null
);
