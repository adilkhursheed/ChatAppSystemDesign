using ChatApp.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SenderController : ControllerBase
    {
        private readonly ILogger<SenderController> _logger;
        private readonly IDeliveryService deliveryService;

        public SenderController(ILogger<SenderController> logger, IDeliveryService deliveryService)
        {
            _logger = logger;
            this.deliveryService = deliveryService;
        }

        [HttpPost("Send")]
        public async Task<IActionResult> Get(Message message)
        {
            await deliveryService.Deliver(message, "");
            return Ok("pass");
        }

        [HttpGet("Ping")]
        public IActionResult Ping(Message message)
        {
            return Ok("Ping");
        }


    }
}
