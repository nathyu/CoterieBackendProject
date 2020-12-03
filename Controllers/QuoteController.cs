using BackendTakeHome.Models;
using BackendTakeHome.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendTakeHome.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuoteController : ControllerBase
    {
        private readonly QuoteContext _context;
        private readonly IQuoteService _quoteService;
        private readonly ILogger<QuoteController> _logger;

        public QuoteController(QuoteContext context, IQuoteService quoteService, ILogger<QuoteController> logger)
        {
            _context = context;
            _quoteService = quoteService;
            _logger = logger;
        }

        // POST: api/quote
        // Returns the total premium.
        [HttpPost]
        public async Task<IActionResult> PostPremium([FromBody] Customer customer)
        {
            var totalPremium = await _quoteService.CalculatePremium(customer);

            if (totalPremium < 0)
            {
                return new NotFoundObjectResult("State or Business not found.");
            }

            return new OkObjectResult(new { premium = totalPremium });
        }

        // POST: api/quote/state
        // Add a list of States to the database. Mostly just a method to setup the database.
        [HttpPost]
        [Route("states")]
        public async Task<IActionResult> PostNewStates([FromBody] List<State> states)
        {
            _context.States.AddRange(states);
            await _context.SaveChangesAsync();

            return new OkObjectResult(states);
        }

        // POST: api/quote/business
        // Add a list of businesses to the database. Mostly just a method to setup the database.
        [HttpPost]
        [Route("businesses")]
        public async Task<IActionResult> PostNewBusinesses([FromBody] List<Business> businesses)
        {
            _context.Businesses.AddRange(businesses);
            await _context.SaveChangesAsync();

            return new OkObjectResult(businesses);
        }
        
    }
}
