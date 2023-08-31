using ChatApp.API.DTO;

namespace ChatApp.API.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private List<Message> Messages { get; set; }
        public MessageRepository()
        {
            this.Messages = new List<Message>();
        }

        public async Task AddMessage(Message msg)
        {
            msg.Id = this.Messages.Count + 1;
            await Task.Run(() => this.Messages.Add(msg));
        }

        public async Task<Message> GetMessage(int messageId)
        {
            return await Task.Run(() => this.Messages.FirstOrDefault(x => x.Id == messageId));
        }
    }
}
