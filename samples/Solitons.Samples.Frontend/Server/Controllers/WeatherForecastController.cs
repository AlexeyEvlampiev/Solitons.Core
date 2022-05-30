using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Solitons.Diagnostics;
using Solitons.Samples.Domain.Contracts;
using Solitons.Samples.Frontend.Shared;

namespace Solitons.Samples.Frontend.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/weather")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes")]
public class WeatherForecastController : ControllerBase
{
    private readonly WeatherForecastCommand _command;
    private readonly IAsyncLogger _logger;

    public WeatherForecastController(WeatherForecastCommand command, IAsyncLogger logger)
    {
        _command = command ?? throw new ArgumentNullException(nameof(command));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }


    [HttpGet]
    public async Task<WeatherForecast[]> Get()
    {
        await _logger.InfoAsync("Weather forecast requested...");
        return await _command
            .InvokeAsync(new WeatherForecastRequest())
            .ToObservable()
            .SelectMany(response => response.Items)
            .Select(item => new WeatherForecast
            {
                Date = item.Date,
                Summary = item.Summary,
                TemperatureC = item.TemperatureC
            })
            .ToArray();
    }
}
