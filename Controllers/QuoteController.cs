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
        // Returns the premium
        [HttpPost]
        public async Task<IActionResult> PostPremium([FromBody]object payload)
        {
            var customer = JsonConvert.DeserializeObject<Customer>(payload.ToString());

            var totalPremium = await _quoteService.CalculatePremium(customer);

            if (totalPremium < 0)
            {
                return new NotFoundObjectResult("State or Business not found.");
            }

            return new OkObjectResult(new { premium = totalPremium });
        }

        // POST: api/quote/state
        // Add a list of States to the database
        [HttpPost]
        [Route("state")]
        public async Task<IActionResult> PostNewStates([FromBody]object state)
        {
            _context.States.AddRange(JsonConvert.DeserializeObject<List<State>>(state.ToString()));
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        // POST: api/quote/business
        // Add a list of businesses to the database
        [HttpPost]
        [Route("business")]
        public async Task<IActionResult> PostNewBusinesses([FromBody]object business)
        {
            _context.Businesses.AddRange(JsonConvert.DeserializeObject<List<Business>>(business.ToString()));
            await _context.SaveChangesAsync();

            return new OkResult();
        }
        
    }
}
