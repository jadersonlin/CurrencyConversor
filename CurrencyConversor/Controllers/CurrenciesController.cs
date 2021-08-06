using CurrencyConversor.Application.Dtos;
using CurrencyConversor.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CurrencyConversor.API.Controllers
{
    [ApiController]
    [Route("api/currencies")]
    public class CurrenciesController : ControllerBase
    {
        private readonly IConversionTransactionService conversionTransactionService;
        private readonly ICurrenciesService currenciesService;
        private readonly ILogger<CurrenciesController> logger;

        public CurrenciesController(IConversionTransactionService conversionTransactionService,
                                    ICurrenciesService currenciesService,
                                    ILogger<CurrenciesController> logger)
        {
            this.conversionTransactionService = conversionTransactionService;
            this.currenciesService = currenciesService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<GetAllCurrenciesResult>> Get()
        {
            var result = await currenciesService.GetAvailableCurrencies();

            return result != null ? Ok(result) : NotFound(nameof(Get));
        }

        [HttpGet]
        [Route("conversion")]
        public async Task<ActionResult<GetConversionResult>> GetConversion(decimal fromValue, string fromCurrency, string toCurrency, string userId)
        {
            var result = await conversionTransactionService.RequestConversion(fromCurrency, toCurrency, fromValue, userId);

            return result != null ? Ok(result) : NotFound(nameof(GetConversion));
        }

        private ActionResult NotFound(string actionName) 
        {
            logger.LogWarning("Conversion not returned in " + actionName);
            return NotFound();
        }
    }
}
