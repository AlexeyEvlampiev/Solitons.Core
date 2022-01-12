using Microsoft.AspNetCore.Mvc;
using Solitons.Samples.Domain.Contracts;
using Solitons.Samples.RestApi.Models;

namespace Solitons.Samples.RestApi.Controllers
{
    [ApiController, Route("api/customers"), ApiVersion("1.0")]
    public class CustomersController : ControllerBase
    {
        private readonly IDatabaseApi _databaseApi;

        public CustomersController(IDatabaseApi databaseApi)
        {
            _databaseApi = databaseApi
                .ThrowIfNullArgument(nameof(databaseApi));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var response = await _databaseApi.InvokeAsync(new CustomerGetRequest(id));
            return Ok(new CustomerData(response));
        }
    }
}
