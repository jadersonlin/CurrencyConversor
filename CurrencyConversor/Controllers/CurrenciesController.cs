using System;
using System.Net;
using System.Net.Http;
using CurrencyConversor.Application.Dtos;
using CurrencyConversor.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CurrencyConversor.API.Controllers
{
    /// <summary>
    /// Conversion os currency in another one.
    /// </summary>
    [ApiController]
    [Route("api/currencies")]
    [Produces("application/json")]
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

        /// <summary>
        /// Get available currencies for conversion.
        /// </summary>
        /// <returns>Available currencies.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetAllCurrenciesResult>> Get()
        {
            var result = await currenciesService.GetAvailableCurrencies();

            return result != null ? Ok(result) : NotFound(nameof(Get));
        }

        /// <summary>
        /// Convert a value from a currency to another and returns conversion data.
        /// </summary>
        /// <param name="fromValue"></param>
        /// <param name="fromCurrency"></param>
        /// <param name="toCurrency"></param>
        /// <param name="userId"></param>
        /// <returns>Conversion data, including the conversion rate and converted value.</returns>
        [HttpGet]
        [Route("conversion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<ActionResult<GetConversionResult>> GetConversion(decimal fromValue, string fromCurrency, string toCurrency, string userId)
        {
            GetConversionResult result = null;

            try
            {
                result = await conversionTransactionService.RequestConversion(fromCurrency, toCurrency, fromValue,
                    userId);
            }
            catch (HttpRequestException ex)
            {
                logger.LogError($"Error in {nameof(GetConversion)}: ", ex);

                if (ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    return BadRequest(ex.Message);
                }

                throw;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in {nameof(GetConversion)}: ", ex);
                throw;
            }

            return result != null ? Ok(result) : NotFound(nameof(GetConversion));
        }

        private ActionResult NotFound(string actionName) 
        {
            logger.LogWarning("Conversion not returned in " + actionName);
            return NotFound();
        }
    }
}
