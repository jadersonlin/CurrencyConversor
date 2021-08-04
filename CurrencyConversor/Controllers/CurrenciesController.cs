using System;
using System.Threading.Tasks;
using CurrencyConversor.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CurrencyConversor.API.Controllers
{
    [ApiController]
    [Route("api/currencies")]
    public class CurrenciesController : ControllerBase
    {
        private readonly ILogger<CurrenciesController> logger;

        public CurrenciesController(ILogger<CurrenciesController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public Task<ActionResult<GetConversionResult>> GetAvailableCurrencies(decimal value, string fromCurrency, string toCurrency, string userId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("conversion")]
        public Task<ActionResult<GetConversionResult>> GetConversion(decimal value, string fromCurrency, string toCurrency, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
