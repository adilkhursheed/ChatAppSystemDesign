namespace ChatApp.API
{
    public class Message
    {
        public int Id { get; set; }
        internal string SenderEmail { get; set; }
        public string ReceiverEmail { get; set; }
        public string Content { get; set; }
        public string ReceiverConnectionId { get; set; }
    }
}
