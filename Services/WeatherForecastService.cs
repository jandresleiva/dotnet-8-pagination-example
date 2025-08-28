
using dotnet_pagination_basics.Services.DTOs;
using System.Text;

namespace dotnet_pagination_basics.Services;

public class WeatherForecastService : IWeatherForecastService
{
  private static readonly string[] Summaries =
  [
      "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
  ];
  // This would probably be a project-based configuration.
  private static readonly int ItemsPerPage = 15;
  private static readonly int TotalItemsToGenerate = 100;

  // You will probably have an async method reaching out to the Database instead of the fixed generation.
  private static readonly IEnumerable<WeatherForecast> WeatherEntries =
    Enumerable.Range(1, TotalItemsToGenerate)
      .Select(index => new WeatherForecast
      {
        Id = index,
        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
      })
      .OrderBy(w => w.Id)
      .ToList();

  public PaginatedResults<WeatherForecast> GetWeatherForecast(int page, bool showTotalCount = false)
  {
    int? resultTotalCount = null;

    if (showTotalCount)
    {
      // In a prod soluction connecting to a database, this would be something like _dbContext.Entity.CountAsync();
      // for the purpose of this example, we will have a fixed 100 total elements.
      int totalItemsToGenerate = 100;
      resultTotalCount = totalItemsToGenerate;
    }

    var items = WeatherEntries
      .Skip(page * ItemsPerPage)
      .Take(ItemsPerPage);

    var paginatedResults = new PaginatedResults<WeatherForecast>(
       items,
       page,
       ItemsPerPage,
       resultTotalCount
   );

    return paginatedResults;
  }

  public KeysetPaginatedResults<WeatherForecast> GetWeatherForecastKeyset(int? lastId = null, int pageSize = 15, bool showTotalCount = false)
  {
    int? resultTotalCount = null;

    if (showTotalCount)
    {
      resultTotalCount = TotalItemsToGenerate;
    }

    var query = WeatherEntries.AsQueryable();

    // If lastId is provided, get records with ID greater than lastId
    if (lastId.HasValue)
    {
      query = query.Where(w => w.Id > lastId.Value);
    }

    var items = query
      .OrderBy(w => w.Id)
      .Take(pageSize + 1) // Take one extra to check if there's a next page
      .ToList();

    var hasNextPage = items.Count > pageSize;
    if (hasNextPage)
    {
      items = items.Take(pageSize).ToList(); // Remove the extra item
    }

    var newLastId = items.LastOrDefault()?.Id;

    return new KeysetPaginatedResults<WeatherForecast>(
      items,
      pageSize,
      newLastId,
      hasNextPage,
      resultTotalCount
    );
  }

  public CursorPaginatedResults<WeatherForecast> GetWeatherForecastCursor(string? cursor = null, int pageSize = 15, bool showTotalCount = false)
  {
    int? resultTotalCount = null;

    if (showTotalCount)
    {
      resultTotalCount = TotalItemsToGenerate;
    }

    var query = WeatherEntries.AsQueryable();
    int? startId = null;

    // Decode cursor if provided
    if (!string.IsNullOrEmpty(cursor))
    {
      try
      {
        var decodedBytes = Convert.FromBase64String(cursor);
        var decodedCursor = Encoding.UTF8.GetString(decodedBytes);
        if (int.TryParse(decodedCursor, out var parsedId))
        {
          startId = parsedId;
          // We don't execute just yet, but prepare the filter
          query = query.Where(w => w.Id > startId.Value);
        }
      }
      catch
      {
        // Invalid cursor, ignore and start from beginning
      }
    }

    var items = query
      .OrderBy(w => w.Id)
      .Take(pageSize + 1) // Take one extra to check if there's a next page
      .ToList();

    var hasNextPage = items.Count > pageSize;
    if (hasNextPage)
    {
      items = items.Take(pageSize).ToList(); // Remove the extra item
    }

    // Generate next cursor
    string? nextCursor = null;
    if (hasNextPage && items.Any())
    {
      var lastId = items.Last().Id;
      var cursorBytes = Encoding.UTF8.GetBytes(lastId.ToString());
      nextCursor = Convert.ToBase64String(cursorBytes);
    }

    // Generate previous cursor
    string? previousCursor = null;
    var hasPreviousPage = startId.HasValue && startId.Value > 1;
    if (hasPreviousPage)
    {
      // For the first page (when going back), we don't need a cursor
      if (startId.Value <= pageSize)
      {
        // Going back to the very first page - no cursor needed (null cursor means start from beginning)
        previousCursor = null;
      }
      else
      {
        // Find the ID that should be the last item of the previous page
        var previousPageLastId = WeatherEntries
          .Where(w => w.Id < startId.Value)
          .OrderByDescending(w => w.Id)
          .Take(pageSize)
          .LastOrDefault()?.Id;

        if (previousPageLastId.HasValue)
        {
          var prevCursorBytes = Encoding.UTF8.GetBytes(previousPageLastId.Value.ToString());
          previousCursor = Convert.ToBase64String(prevCursorBytes);
        }
      }
    }

    return new CursorPaginatedResults<WeatherForecast>(
      items,
      pageSize,
      nextCursor,
      previousCursor,
      hasNextPage,
      hasPreviousPage,
      resultTotalCount
    );
  }
}