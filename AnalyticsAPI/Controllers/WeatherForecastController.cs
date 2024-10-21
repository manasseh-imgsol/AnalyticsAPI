using AnalyticsAPI.Analytics.Services;
using Microsoft.AspNetCore.Mvc;

namespace AnalyticsAPI.Controllers;



[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly AnalyticsService _analyticsService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, AnalyticsService analyticsService)
    {
        _logger = logger;
        _analyticsService = analyticsService;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        await _analyticsService.LogRequestAsync("/weatherforecast", true, 300);

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
