
using ChatApp.API.Channel;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.API.Services
{
    public class DiscoverService : IDiscoverService
    {
        private readonly IHubContext<MessageHub> messageHubContext;

        public DiscoverService(IHubContext<MessageHub> messageHubContext)
        {
            this.messageHubContext = messageHubContext;
        }
        public async Task<string> DiscoverChanel(string connecctionId)
        {
            if (this.messageHubContext.Clients.Client(connecctionId)==null)
                return "";//means receiver is on the same hub
            return "https://localhost:7126";
        }
    }
}
