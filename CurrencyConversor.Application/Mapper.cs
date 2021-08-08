using System;
using CurrencyConversor.Application.Dtos;
using CurrencyConversor.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyConversor.Application
{
    public static class Mapper
    {
        public static GetAllSuccessTransactionsResult MapSuccessfulTransactions(IList<SuccessTransaction> transactions)
        {
            return new GetAllSuccessTransactionsResult
            {
                Transactions = transactions.Select(t => new SuccessTransactionDto
                {
                    Id = t.Id,
                    FromValue = t.FromValue.Value,
                    FromCurrency = t.FromCurrency,
                    ToCurrency = t.ToCurrency,
                    ConversionTimestamp = DateTimeOffset.FromUnixTimeSeconds(t.ConversionTimestamp.Value).UtcDateTime,
                    UserId = t.UserId,
                    ConversionRate = t.ConversionRate
                }).ToList(),
                Message = "Success!"
            };
        }

        public static GetAllFailureTransactionsResult MapFailedTransactions(IList<FailureTransaction> transactions)
        {

            return new GetAllFailureTransactionsResult
            {
                Transactions = transactions.Select(t => new FailureTransactionDto
                {
                    Id = t.Id,
                    FromValue = t.FromValue,
                    FromCurrency = t.FromCurrency,
                    ToCurrency = t.ToCurrency,
                    ConversionTimestamp = DateTimeOffset.FromUnixTimeSeconds(t.ConversionTimestamp.Value).UtcDateTime,
                    ErrorMessage = t.ErrorMessage,
                    UserId = t.UserId
                }).ToList(),
                Message = "Success!"
            };
        }

        public static GetConversionResult MapSuccessConversion(SuccessTransaction transaction)
        {
            return new GetConversionResult
            {
                Id = transaction.Id,
                FromValue = transaction.FromValue.Value,
                FromCurrency = transaction.FromCurrency,
                ToCurrency = transaction.ToCurrency,
                ConversionRate = transaction.ConversionRate,
                ConversionTimestamp = DateTimeOffset.FromUnixTimeSeconds(transaction.ConversionTimestamp.Value).DateTime,
                UserId = transaction.UserId,
                Message = "Sucess!"
            };
        }

        public static GetAllCurrenciesResult MapCurrencies(IList<Currency> currencies)
        {
            var currencyList = currencies.Select(c => new CurrencyDto
            {
                Code = c.Code,
                Name = c.Name
            }).ToList();

            return new GetAllCurrenciesResult
            {
                Currencies = currencyList,
                Message = "Success!"
            };
        }
    }
}
