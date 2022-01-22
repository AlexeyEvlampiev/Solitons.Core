using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Solitons.Samples.Frontend.Client.Pages
{
    [Authorize]
    public partial class SecuredImageViewer
    {
        [Inject]
        protected HttpClient? Http { get; set; }

        protected Uri? ImageUri { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Debug.Assert(Http is not null);
            try
            {
                var result = await Http.GetStringAsync(new Uri("api/images/e8d6ee5e-f469-48ce-b794-5aa3ddc05447/source", UriKind.Relative));
                ImageUri = new Uri(result);
            }
            catch (AccessTokenNotAvailableException exception)
            {
                exception.Redirect();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
