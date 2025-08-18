namespace dotnet_pagination_basics.Services.DTOs;

public record PaginatedResults<T>(
  IEnumerable<T> Items,
  int Page,
  int PageSize,
  int? TotalCount
);