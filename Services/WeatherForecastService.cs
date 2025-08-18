
using dotnet_pagination_basics.Services.DTOs;

namespace dotnet_pagination_basics.Services;

public class WeatherForecastService: IWeatherForecastService
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
        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
      })
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
}