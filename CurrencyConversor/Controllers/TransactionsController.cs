using CurrencyConversor.Application.Dtos;
using CurrencyConversor.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetAllSuccessTransactionsResult>> GetSuccessTransactions()
        {
            var result = await conversionTransactionService.GetAllSuccessfulTransactions();

            if (result == null)
            {
                logger.LogWarning("Transactions not found in " + nameof(GetSuccessTransactions));

                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("failures")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetAllFailureTransactionsResult>> GetFailureTransactions()
        {
            var result = await conversionTransactionService.GetAllFailedTransactions();

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
