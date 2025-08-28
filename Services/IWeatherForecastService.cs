using dotnet_pagination_basics.Services.DTOs;

namespace dotnet_pagination_basics.Services;

public interface IWeatherForecastService
{
  public PaginatedResults<WeatherForecast> GetWeatherForecast(int page, bool getTotalCount);
  
  public KeysetPaginatedResults<WeatherForecast> GetWeatherForecastKeyset(int? lastId = null, int pageSize = 15, bool showTotalCount = false);
  
  public CursorPaginatedResults<WeatherForecast> GetWeatherForecastCursor(string? cursor = null, int pageSize = 15, bool showTotalCount = false);
}