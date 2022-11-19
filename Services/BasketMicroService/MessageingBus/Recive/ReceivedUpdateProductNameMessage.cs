using BasketMicroService.Model.Services.ProductServices;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqMessageBus.MessagingBus;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BasketMicroService.MessageingBus
{
    public class ReceivedUpdateProductNameMessage : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;
        private readonly string _hostname;
        private readonly string _username;
        private readonly string _password;
        private readonly string _queueName;
        private readonly string _exchangeName;
        private readonly IProductService productService;
        public ReceivedUpdateProductNameMessage(IOptions<RabbitMqConfiguration> rabbitMqOptions
            , IProductService productService)
        {
            _hostname = rabbitMqOptions.Value.Hostname;
            _exchangeName = rabbitMqOptions.Value.ExchangeName_UpdateProductName;
            _username = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;
            _queueName = rabbitMqOptions.Value.QueueName_GetMessageonUpdateProductName;
            this.productService = productService;
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(_exchangeName, ExchangeType.Fanout, true, false);
            _channel.QueueDeclare(_queueName, true, false, false);
            _channel.QueueBind(_queueName, _exchangeName, "");



        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var updateCustomerFullNameModel = JsonConvert.DeserializeObject<UpdateProductNameMessage>(content);

                var resultHandeleMessage = HandleMessage(updateCustomerFullNameModel);
                if (resultHandeleMessage)
                    _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(_queueName, false, consumer);
            return Task.CompletedTask;
        }


        private bool HandleMessage(UpdateProductNameMessage updateProduct)
        {
            return productService.UpdateProductName(updateProduct.Id, updateProduct.NewName);
        }
    }


    public class UpdateProductNameMessage
    {
        public Guid Id { get; set; }
        public string NewName { get; set; }
    }
}
