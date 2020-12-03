using BackendTakeHome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendTakeHome.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly QuoteContext _context;
        
        public QuoteService(QuoteContext context)
        {
            _context = context;
        }

        public async Task<double> CalculatePremium(Customer customer)
        {
            if (_context.States.Find(customer.State) == null || _context.Businesses.Find(customer.Business) == null)
            {
                return -1;
            }

            var stateFactor = _context.States.Find(customer.State).Factor;
            var businessFactor = _context.Businesses.Find(customer.Business).Factor;
            var basePremium = Math.Ceiling((double)customer.Revenue / 1000);
            var hazardFactor = 4;

            return stateFactor * businessFactor * basePremium * hazardFactor;
        }
    }
}
