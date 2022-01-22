using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Solitons.Reflection;
using Solitons.Samples.Domain;
using Solitons.Samples.Domain.Contracts;

namespace Solitons.Samples.Frontend.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/images")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes")]
    public class ImagesController : ControllerBase
    {
        private readonly ISampleDbApi _databaseApi;
        private readonly RecursivePropertyInspector _inspector;
        private readonly IAsyncLogger _logger;

        public ImagesController(ISampleDbApi databaseApi, RecursivePropertyInspector inspector, IAsyncLogger logger)
        {
            _databaseApi = databaseApi ?? throw new ArgumentNullException(nameof(databaseApi));
            _inspector = inspector ?? throw new ArgumentNullException(nameof(inspector));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet(), Route("{oid}")]
        public async Task<IActionResult> GetAsync(Guid oid)
        {
            var request = new ImageGetRequest(oid);
            var response = await _databaseApi.InvokeAsync(request);
            await _inspector.InspectAsync(response);
            return Redirect(response.ImageSource ?? string.Empty);
        }

        [HttpGet(), Route("{oid}/source")]
        public async Task<string> GetSourceAsync(Guid oid)
        {
            var request = new ImageGetRequest(oid);
            var response = await _databaseApi.InvokeAsync(request);
            await _inspector.InspectAsync(response);
            return response.ImageSource ?? String.Empty;
        }
    }
}
