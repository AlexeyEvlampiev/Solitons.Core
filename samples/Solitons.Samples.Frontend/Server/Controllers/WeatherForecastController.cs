using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Solitons.Samples.Domain;
using Solitons.Samples.Domain.Contracts;
using Solitons.Samples.Frontend.Shared;

namespace Solitons.Samples.Frontend.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/weather")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes")]
public class WeatherForecastController : ControllerBase
{
    private readonly ISampleDbApi _databaseApi;
    private readonly IAsyncLogger _logger;

    public WeatherForecastController(ISampleDbApi databaseApi, IAsyncLogger logger)
    {
        _databaseApi = databaseApi ?? throw new ArgumentNullException(nameof(databaseApi));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }


    [HttpGet]
    public async Task<WeatherForecast[]> Get()
    {
        await _logger.InfoAsync("Weather forecast requested...");
        return await _databaseApi
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