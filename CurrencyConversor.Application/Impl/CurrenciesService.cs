using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyConversor.Application.Dtos;
using CurrencyConversor.Application.Interfaces;
using CurrencyConversor.Domain.Repositories;

namespace CurrencyConversor.Application.Impl
{
    public class CurrenciesService : ICurrenciesService
    {
        private readonly ICurrencyRepository currencyRepository;

        public CurrenciesService(ICurrencyRepository currencyRepository)
        {
            this.currencyRepository = currencyRepository;
        }

        public async Task<GetAllCurrenciesResult> GetAvailableCurrencies()
        {
            var currencies = await currencyRepository.GetAll();

            return Mapper.MapCurrencies(currencies);
        }
    }
}
