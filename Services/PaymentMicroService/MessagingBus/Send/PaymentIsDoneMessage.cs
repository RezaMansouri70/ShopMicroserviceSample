using RabbitMqMessageBus.MessagingBus;

namespace PaymentMicroService.MessagingBus.Send
{
    public class PaymentIsDoneMessage : BaseMessage
    {
        public Guid OrderId { get; set; }

    }
}
