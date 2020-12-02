using BackendTakeHome.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace BackendTakeHome.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuoteController : ControllerBase
    {
        private readonly QuoteContext _context;
        private readonly ILogger<QuoteController> _logger;

        public QuoteController(QuoteContext context, ILogger<QuoteController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return "Hello World";
        }

        [HttpPost]
        [Route("state")]
        public async Task<IActionResult> AddState([FromBody] object state)
        {
            _context.States.Add(JsonConvert.DeserializeObject<State>(state.ToString()));
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        [HttpPost]
        [Route("business")]
        public async Task<IActionResult> AddBusiness([FromBody] object business)
        {
            _context.Businesses.Add(JsonConvert.DeserializeObject<Business>(business.ToString()));
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        [HttpPost]
        public JsonResult Post([FromBody] object payload)
        {
            var customer = JsonConvert.DeserializeObject<Customer>(payload.ToString());

            var totalPremium = CalculateTotalPremium(customer);
            return new JsonResult(totalPremium);
        }

        private double CalculateTotalPremium(Customer customer)
        {
            var stateFactor = _context.States.Find(customer.State).Factor;
            var businessFactor = _context.Businesses.Find(customer.Business).Factor;
            var basePremium = Math.Ceiling((double) customer.Revenue / 1000);
            var hazardFactor = 4;

            return stateFactor * businessFactor * basePremium * hazardFactor;
        }
    }
}
