using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConversor.Domain.Models
{
    public class CurrencyParameters
    {
        public Currency FromCurrency { get; set; }

        public Currency ToCurrency { get; set; }

        public decimal FromValue { get; set; }

        public string UserId { get; set; }
    }
}
