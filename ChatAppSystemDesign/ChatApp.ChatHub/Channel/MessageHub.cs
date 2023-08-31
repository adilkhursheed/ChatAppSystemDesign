using ChatApp.API.DTO;
using ChatApp.API.Repository;
using ChatApp.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.API.Channel
{
    [Authorize(AuthenticationSchemes = "HubAuth")]
    public class MessageHub : Hub
    {
        public const string ReceiveMessage = "ReceiveMessage";
        public const string BroadCastNotification = "BroadCastNotificaiton";
        private readonly ISenderService senderService;
        private readonly IUserRepository userRepository;

        public MessageHub(ISenderService senderService, IUserRepository userRepository)
        {
            this.senderService = senderService;
            this.userRepository = userRepository;
        }

        public async Task BroadCastNotificaiton(string notification)
        {
            //// Send via sender service to deliver to all server hubs
            await Clients.AllExcept(Context.ConnectionId).SendAsync(BroadCastNotification, notification);
        }
        public async Task SendMessage(Message message)
        {
            message.SenderEmail = Context.User?.Identity?.Name;
            if (!string.IsNullOrEmpty(message.SenderEmail))
            {
                await this.senderService.Send(message);
            }
            else
            {
                await Clients.Caller.SendAsync(ReceiveMessage, "You are not logged in correctly, please login again.");
            }
        }
        //internal async Task DeliverMessage(Message message)
        //{
        //    await Clients.User(message.ReceiverEmail).SendAsync(ReceiveMessage, message.Content);
        //}
        public async override Task OnConnectedAsync()
        {
            if (Context.User?.Identity?.IsAuthenticated ?? false)
            {
                var user= new UserDto()
                {
                    UserName= Context.User.Identity.Name,
                    ChannelId= Context.ConnectionId
                };
                await this.userRepository.AddActiveUser(user);
            }
        }

    }
}
