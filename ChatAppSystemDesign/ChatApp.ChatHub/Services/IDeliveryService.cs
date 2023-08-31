namespace ChatApp.API.Services
{
    public interface IDeliveryService
    {
        Task Deliver(Message message, string host);
    }
}
