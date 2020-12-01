using BackendTakeHome.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
        public void AddState([FromBody] object state)
        {
            _context.States.Add(JsonConvert.DeserializeObject<State>(state.ToString()));
        }

        [HttpPost]
        public JsonResult Post([FromBody] object payload)
        {
            var customer = JsonConvert.DeserializeObject<Customer>(payload.ToString());


            return new JsonResult(CalculateTotalPremium(customer));
        }

        private int CalculateTotalPremium(Customer customer)
        {
            var stateFactor = _context.States.Find("TX");
            _logger.LogInformation(stateFactor.ToString());
            return 0;
        }
    }
}
