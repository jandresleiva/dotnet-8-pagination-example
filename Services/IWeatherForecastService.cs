
using dotnet_pagination_basics.Services.DTOs;

namespace dotnet_pagination_basics.Services;

public interface IWeatherForecastService
{
  public PaginatedResults<WeatherForecast> GetWeatherForecast(int page, bool getTotalCount);
}