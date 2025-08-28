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

    [HttpGet("keyset")]
    public IActionResult GetKeysetPagination([FromQuery] int? lastId = null, [FromQuery] int pageSize = 15, [FromQuery] bool getTotalCount = false)
    {
        var result = _weatherForecastService.GetWeatherForecastKeyset(lastId, pageSize, getTotalCount);
        return Ok(result);
    }

    [HttpGet("cursor")]
    public IActionResult GetCursorPagination([FromQuery] string? cursor = null, [FromQuery] int pageSize = 15, [FromQuery] bool getTotalCount = false)
    {
        var result = _weatherForecastService.GetWeatherForecastCursor(cursor, pageSize, getTotalCount);
        return Ok(result);
    }
}
