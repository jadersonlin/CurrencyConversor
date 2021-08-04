using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConversor.Application.Interfaces
{
    public  interface IAuthService
    {
        Task UserExists(string userId);
    }
}
