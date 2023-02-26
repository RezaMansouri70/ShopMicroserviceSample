using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OrderMicroService.Infrastructure.Context;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqMessageBus.MessagingBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderMicroService.MessagingBus
{
    public class RecievedPaymentOfOrderService : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;
        private readonly string _hostname;
        private readonly string _queueName;
        private readonly string _username;
        private readonly string _password;
        private readonly OrderDataBaseContext context;

        public RecievedPaymentOfOrderService(OrderDataBaseContext context, IOptions<RabbitMqConfiguration>
            rabbitMqOptions)
        {
            _hostname = rabbitMqOptions.Value.Hostname;
            _queueName = rabbitMqOptions.Value.QueueName_PaymentDone;
            _username = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName, durable: true,
                exclusive: false,
                autoDelete: false, arguments: null);
            this.context = context;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
             {
                 var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                 var paymentDone = JsonConvert.
                 DeserializeObject<PaymentOrderMessage>(content);
                 var resultHandeleMessage = HandleMessage(paymentDone);
                 if (resultHandeleMessage)
                     _channel.BasicAck(ea.DeliveryTag, false);

             };

            _channel.BasicConsume(_queueName, false, consumer);
            return Task.CompletedTask;

        }
        private bool HandleMessage(PaymentOrderMessage paymentOrderMessage)
        {
            var order = context.Orders.SingleOrDefault(p => p.Id == paymentOrderMessage.OrderId);
            if (order != null)
            {
                order.PaymentIsDone();
                context.SaveChanges();
            }
            return true;
        }


    }

    public class PaymentOrderMessage
    {
        public Guid OrderId { get; set; }
    }
}
