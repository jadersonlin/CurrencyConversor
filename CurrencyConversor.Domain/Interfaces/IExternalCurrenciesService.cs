using System.Threading.Tasks;
using CurrencyConversor.Domain.Models;

namespace CurrencyConversor.Domain.Interfaces
{
    public interface IExternalCurrenciesService
    {
        Task<ExternalConversionData> GetConversionRate(string fromCurrency, string toCurrency);
    }
}
