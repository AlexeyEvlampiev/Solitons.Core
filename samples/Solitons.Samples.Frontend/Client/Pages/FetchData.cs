using System.Net.Http.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Solitons.Samples.Frontend.Shared;

namespace Solitons.Samples.Frontend.Client.Pages
{
    [Authorize]
    public partial class FetchData
    {
        [Inject]
        protected HttpClient? Http { get; set; }

        private WeatherForecast[]? _forecasts;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _forecasts = await Http.GetFromJsonAsync<WeatherForecast[]>("WeatherForecast");
            }
            catch (AccessTokenNotAvailableException exception)
            {
                exception.Redirect();
            }
        }
    }
}
