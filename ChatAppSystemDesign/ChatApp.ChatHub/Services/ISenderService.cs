namespace ChatApp.API.Services
{
    public interface ISenderService
    {
        public Task Send(Message message);
    }
}
