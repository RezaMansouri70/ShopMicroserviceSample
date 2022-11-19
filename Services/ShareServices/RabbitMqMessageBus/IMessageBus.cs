namespace RabbitMqMessageBus.MessagingBus
{
    public interface IMessageBus
    {
        void SendMessage(BaseMessage message, string exchange,string queueName);
    }

}
