using CurrencyConversor.Domain.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;

namespace CurrencyConversor.Infraestructure.MongoDB
{
    public class CurrencyContext : ICurrencyContext
    {
        public CurrencyContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetSection("MongoDb:ConnectionString").Value);
            var database = client.GetDatabase(configuration.GetSection("MongoDb:Database").Value);

            Currencies = database.GetCollection<Currency>("products");
            SuccessTransactions = database.GetCollection<SuccessTransaction>("success");
            FailureTransactions = database.GetCollection<FailureTransaction>("failure");

            PopulateAvailableCurrencies();
        }

        public IMongoCollection<Currency> Currencies { get; }
        public IMongoCollection<SuccessTransaction> SuccessTransactions { get; }
        public IMongoCollection<FailureTransaction> FailureTransactions { get; }

        private void PopulateAvailableCurrencies()
        {
            var currencies = new List<Currency>
            {
                new Currency("ARS", "Argentinian Peso"),
                new Currency("AUD", "Australian Dollar"),
                new Currency("BOB", "Bolivian"),
                new Currency("BRL", "Real"),
                new Currency("CNY", "Renminbi"),
                new Currency("EGP", "Egyptian Pound"),
                new Currency("EUR", "Euro"),
                new Currency("GBP", "Pound Sterling"),
                new Currency("ILS", "Shekel"),
                new Currency("JPY", "Yen"),
                new Currency("SEK", "Sweden Crown"),
                new Currency("MXN", "Mexican Peso"),
                new Currency("RUB", "Russian Ruble"),
                new Currency("TRY", "Turkish Lira"),
            };

            var hasItems = Currencies.Find(c => true).Any();

            if (!hasItems)
                Currencies.InsertMany(currencies);
        }
    }
}
