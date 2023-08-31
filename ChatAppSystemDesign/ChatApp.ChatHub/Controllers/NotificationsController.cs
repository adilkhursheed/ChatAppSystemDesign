using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ChatApp.API.Channel;

namespace ChatApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IHubContext<NotificationsHub> hubContext;
        static int increment = 0;
        public NotificationsController(IHubContext<NotificationsHub> hubContext)
        {
            this.hubContext = hubContext;
        }
        [HttpGet("NotifyAll/{message}")]
        public async Task<IActionResult> NotifyAll(string message)
        {
            await hubContext.Clients.All.SendAsync("BroadCastNotificaiton", $"Message:{increment++} | {message}");
            //(new NotificationsHub()).BroadCastNotificaiton(message);
            return Ok(message);
        }
    }
}
