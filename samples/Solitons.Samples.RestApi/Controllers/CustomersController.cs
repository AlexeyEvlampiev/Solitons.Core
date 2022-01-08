using Microsoft.AspNetCore.Mvc;
using Solitons.Samples.Domain.Contracts;
using Solitons.Samples.RestApi.Models;

namespace Solitons.Samples.RestApi.Controllers
{
    [ApiController, Route("api/customers"), ApiVersion("1.0")]
    public class CustomersController : ControllerBase
    {
        private readonly ITransactionScriptApi _transactionScriptApi;

        public CustomersController(ITransactionScriptApi transactionScriptApi)
        {
            _transactionScriptApi = transactionScriptApi
                .ThrowIfNullArgument(nameof(transactionScriptApi));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var response = await _transactionScriptApi.InvokeAsync(new CustomerGetRequest(id));
            return Ok(new CustomerData(response));
        }
    }
}
