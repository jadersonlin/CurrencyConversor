using System;
using System.Threading.Tasks;
using CurrencyConversor.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConversor.API.Controllers
{
    [Route("transactions")]
    [ApiController]
    [Produces("application/json")]
    public class TransactionsController : ControllerBase
    {
        [HttpGet]
        public Task<ActionResult<GetTransactionsResult>> Get()
        {
            throw new NotImplementedException();
        }
    }
}
