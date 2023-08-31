namespace ChatApp.API.Repository
{
    public interface IMessageRepository
    {
        Task AddMessage(Message msg);
        Task<Message> GetMessage(int messageId);
    }
}