using CurrencyConversor.Application.Dtos;
using System.Threading.Tasks;

namespace CurrencyConversor.Application.Interfaces
{
    public interface ICurrenciesService
    {
        Task<GetAllCurrenciesResult> GetAvailableCurrencies();
    }
}
