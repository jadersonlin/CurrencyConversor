using System;
using System.Net;
using System.Threading.Tasks;
using CurrencyConversor.Application.Dtos;
using CurrencyConversor.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CurrencyConversor.API.Controllers
{
    [Route("transactions")]
    [ApiController]
    [Produces("application/json")]
    public class TransactionsController : ControllerBase
    {
        private readonly IConversionTransactionService conversionTransactionService;
        private readonly ILogger<TransactionsController> logger;

        public TransactionsController(IConversionTransactionService conversionTransactionService,
                                      ILogger<TransactionsController> logger)
        {
            this.conversionTransactionService = conversionTransactionService;
            this.logger = logger;
        }

        [HttpGet]
        [Route("success")]
        public async Task<ActionResult<GetAllSuccessTransactionsResult>> GetSuccessTransactions()
        {
            var result = await conversionTransactionService.GetAllSuccessfulTransactions();

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
