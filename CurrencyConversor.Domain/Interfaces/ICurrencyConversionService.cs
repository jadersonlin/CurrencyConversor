using CurrencyConversor.Domain.Abstractions;
using CurrencyConversor.Domain.Models;
using System.Threading.Tasks;

namespace CurrencyConversor.Domain.Interfaces
{
    public interface ICurrencyConversionService
    {
        Task<ConversionTransaction> Convert(Currency fromCurrency, Currency toCurrency, decimal fromValue, string userId);
    }
}
