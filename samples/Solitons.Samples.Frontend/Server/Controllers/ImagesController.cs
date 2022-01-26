﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Solitons.Samples.Domain;
using Solitons.Samples.Domain.Contracts;
using Solitons.Security;

namespace Solitons.Samples.Frontend.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/images")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes")]
    public class ImagesController : ControllerBase
    {
        private readonly ISampleDbApi _databaseApi;
        private readonly ISecureBlobAccessUriBuilder _secureBlobAccessUriBuilder;
        private readonly IAsyncLogger _logger;

        public ImagesController(ISampleDbApi databaseApi, ISecureBlobAccessUriBuilder secureBlobAccessUriBuilder, IAsyncLogger logger)
        {
            _databaseApi = databaseApi ?? throw new ArgumentNullException(nameof(databaseApi));
            _secureBlobAccessUriBuilder = secureBlobAccessUriBuilder ?? throw new ArgumentNullException(nameof(secureBlobAccessUriBuilder));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet(), Route("{oid}")]
        public async Task<IActionResult> GetAsync(Guid oid)
        {
            var request = new ImageGetRequest(oid);
            var response = await _databaseApi.InvokeAsync(request);
            var ip = Request.HttpContext.Connection.RemoteIpAddress;
            
            var uri = _secureBlobAccessUriBuilder
                .BuildDownloadUri(
                    response.ImageRelativePath,
                    response.AccessTimeWindow,
                    response.AllowAllIpAddresses ? null : ip);
            return Redirect(uri.ToString());
        }

        [HttpGet(), Route("{oid}/source")]
        public async Task<string> GetSourceAsync(Guid oid)
        {
            var request = new ImageGetRequest(oid);
            var response = await _databaseApi.InvokeAsync(request);
            var ip = Request.HttpContext.Connection.RemoteIpAddress;

            var uri = _secureBlobAccessUriBuilder
                .BuildDownloadUri(
                    response.ImageRelativePath,
                    response.AccessTimeWindow,
                    response.AllowAllIpAddresses ? null : ip);
            return uri.ToString();
        }
    }
}
