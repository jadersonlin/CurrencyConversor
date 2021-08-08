using CurrencyConversor.Domain.Interfaces;
using CurrencyConversor.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace CurrencyConversor.Infraestructure.External
{
    public class ExternalCurrenciesService : IExternalCurrenciesService
    {
        private readonly ILogger<ExternalCurrenciesService> logger;
        private readonly string apiUrl;
        private const string BaseCurrency = "USD";

        public ExternalCurrenciesService(IConfiguration configuration, ILogger<ExternalCurrenciesService> logger)
        {
            this.logger = logger;
            var accessKey = configuration.GetSection("CurrencyLayerAPI:AccessKey").Value;
            apiUrl = configuration.GetSection("CurrencyLayerAPI:Url").Value.Replace("{AccessKey}", accessKey);
        }

        public async Task<ExternalConversionData> GetConversionRate(string fromCurrency, string toCurrency)
        {
            var url = apiUrl
                .Replace("{CommaSeparatedCurrencies}", fromCurrency + "," + toCurrency)
                .Replace("{SourceCurrency}", BaseCurrency);
            
            IRestResponse<ApiLayerExchangeSuccessData> response = null;
            ApiLayerExchangeSuccessData responseData = null;

            try
            {
                response = await GetResponse(url);
                responseData = JsonConvert.DeserializeObject<ApiLayerExchangeSuccessData>(response.Content);
            }
            catch (Exception ex)
            {
                logger.LogError("Error on conversion API response.", ex);
            }

            if (responseData == null || !responseData.Success)
            {

                ReportError(fromCurrency, toCurrency, response);
                return null;
            }

            return new ExternalConversionData
            {
                BaseCurrency = BaseCurrency,
                FromCurrency = responseData?.Source,
                ToCurrency = toCurrency,
                BaseFromConversionRate = responseData.Quotes[$"{BaseCurrency}{fromCurrency}"],
                BaseToConversionRate = responseData.Quotes[$"{BaseCurrency}{toCurrency}"],
                ConversionTimestamp = responseData.Timestamp
            };
        }

        private void ReportError(string fromCurrency, string toCurrency, IRestResponse response)
        {
            var message = "Error trying to get conversion rate in external API.\r\n"
                          + "Request data: \r\n"
                          + $"Source: {fromCurrency} \r\n"
                          + $"To: {toCurrency} \r\n"
                          + $"Response: {response?.Content}";

            logger.LogError(message);

            throw new ExternalException(message);
        }

        private static async Task<IRestResponse<ApiLayerExchangeSuccessData>> GetResponse(string url)
        {
            var restClient = new RestClient(url);
            var request = new RestRequest(Method.GET);

            return await restClient.ExecuteAsync<ApiLayerExchangeSuccessData>(request);
        }
    }
}
