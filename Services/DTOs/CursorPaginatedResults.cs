namespace dotnet_pagination_basics.Services.DTOs;

public record CursorPaginatedResults<T>(
    IEnumerable<T> Items,
    int PageSize,
    string? NextCursor,
    string? PreviousCursor,
    bool HasNextPage,
    bool HasPreviousPage,
    int? TotalCount = null
);
