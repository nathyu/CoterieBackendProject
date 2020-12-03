using BackendTakeHome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendTakeHome.Services
{
    public interface IQuoteService
    {
        Task<double> CalculatePremium(Customer customer);
    }
}
