using ChatApp.API.Repository;

namespace ChatApp.API.Services
{
    public class SenderService : ISenderService
    {
        private readonly ILogger<SenderService> logger;
        private readonly IMessageRepository messageRepository;
        private readonly IUserRepository userRepository;
        private readonly IDiscoverService discoverService;
        private readonly IDeliveryService deliveryService;

        public SenderService(ILogger<SenderService> logger, IMessageRepository messageRepository, IUserRepository userRepository,
            IDiscoverService discoverService, IDeliveryService deliveryService)
        {
            this.logger = logger;
            this.messageRepository = messageRepository;
            this.userRepository = userRepository;
            this.discoverService = discoverService;
            this.deliveryService = deliveryService;
        }

        public async Task Send(Message message)
        {
            var recepient= await this.userRepository.GetActiveUser(message.ReceiverEmail);
            message.ReceiverConnectionId = recepient.ChannelId;
            if (recepient == null)
            {
                await this.messageRepository.AddMessage(message);
            }
            else
            {
                var recepientHost=  await this.discoverService.DiscoverChanel(message.ReceiverConnectionId);

                await this.deliveryService.Deliver(message, recepientHost);
            }
        }
    }
}
