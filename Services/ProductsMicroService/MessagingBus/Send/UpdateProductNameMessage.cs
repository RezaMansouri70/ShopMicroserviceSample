using System;
using RabbitMqMessageBus.MessagingBus;

namespace ProductsMicroService.MessagingBus
{
    public class UpdateProductNameMessage : BaseMessage
    {
        public Guid Id { get; set; }
        public string NewName { get; set; }
    }
}
