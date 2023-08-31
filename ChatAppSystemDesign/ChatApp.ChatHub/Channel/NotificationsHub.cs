using Microsoft.AspNetCore.SignalR;

namespace ChatApp.API.Channel
{
    public class NotificationsHub : Hub
    {
        public NotificationsHub() { }

        public async Task BroadCastNotificaiton1(string notification)
        {
            await Clients.AllExcept(Context.ConnectionId).SendAsync("BroadCastNotificaiton1", notification);
        }
    }
}
