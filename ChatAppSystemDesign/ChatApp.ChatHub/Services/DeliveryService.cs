using ChatApp.API.Channel;
using Microsoft.AspNetCore.SignalR;
using System.Net.Http.Headers;

namespace ChatApp.API.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IHubContext<MessageHub>  hubContext;

        public DeliveryService(IHubContext<MessageHub> hubContext)
        {
            this.hubContext = hubContext;
        }
        public async Task Deliver(Message message, string host)
        {
            if (string.IsNullOrEmpty(host))
            {
                // means recipient is on the same hub
                await this.hubContext.Clients.Client(message.ReceiverConnectionId)
                    .SendAsync(MessageHub.ReceiveMessage, message);
            }
            else
            {
                //await this.hubContext.Clients.Client(message.ReceiverConnectionId)
                //    .SendAsync(MessageHub.ReceiveMessage, message);
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(host);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //GET Method
                    HttpResponseMessage response = await client.PostAsJsonAsync("Sender/Send",message);
                    if (response.IsSuccessStatusCode)
                    {
                        
                    }
                }

            }
        }
    }
}
