using dotnet_pagination_basics.Services;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_pagination_basics.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IWeatherForecastService _weatherForecastService;

    public WeatherForecastController(IWeatherForecastService weatherForecastService)
    {
        _weatherForecastService = weatherForecastService;        
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IActionResult Get(int? pageNumber, bool? getTotalCount)
    {
        var page = pageNumber ?? 0;

        var result = _weatherForecastService.GetWeatherForecast(page, getTotalCount ?? false);

        return Ok(result);
    }
}
