using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OrderMicroService.Model.Services.RegisterOrderServices;
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
    public class RecievedOrderCreatedMessage : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;

        private readonly string _hostname;
        private readonly string _queueName;
        private readonly string _username;
        private readonly string _password;
        private readonly IRegisterOrderService registerOrderService;

        public RecievedOrderCreatedMessage(IOptions<RabbitMqConfiguration> rabbitMqOptions,
            IRegisterOrderService registerOrderService)
        {

            _hostname = rabbitMqOptions.Value.Hostname;
            _queueName = rabbitMqOptions.Value.QueueName_BasketCheckout;
            _username = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;

            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.QueueDeclare(queue: _queueName, durable: true,
                    exclusive: false, autoDelete: false, arguments: null);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"cant create connection :{DateTime.Now} {ex.Message}");
            }

            this.registerOrderService = registerOrderService;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            Console.WriteLine($"get basket at {DateTime.Now}");
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (sender, eventArg) =>
             {
                 var body = Encoding.UTF8.GetString(eventArg.Body.ToArray());
                 var basket = JsonConvert.DeserializeObject<BasketDto>(body);
                 Console.WriteLine($"Received at {DateTime.Now}  BasketId:{basket.BasketId}");

                 //ثبت سفارش
                 var resultHandle = HandleMessage(basket);

                 if (resultHandle)
                     _channel.BasicAck(eventArg.DeliveryTag, false);
             };
            _channel.BasicConsume(queue: _queueName, false, consumer);


            return Task.CompletedTask;
        }


        private bool HandleMessage(BasketDto basket)
        {
            return registerOrderService.Execute(basket);
        }


    }



    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class BasketItem
    {
        public string BasketItemId { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }

    public class BasketDto
    {
        public string BasketId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string UserId { get; set; }
        public int TotalPrice { get; set; }
        public List<BasketItem> BasketItems { get; set; }
        public string MessageId { get; set; }
        public DateTime Creationtime { get; set; }
    }




}
