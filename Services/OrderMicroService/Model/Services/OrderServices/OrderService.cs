using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OrderMicroService.Infrastructure.Context;
using RabbitMqMessageBus.MessagingBus;

namespace OrderMicroService.Model.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderDataBaseContext context;
        private readonly IMessageBus messageBus;
        private readonly string QueueName_OrderSendToPayment;
        public OrderService(OrderDataBaseContext context, IMessageBus messageBus,
            IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            this.context = context;
            this.messageBus = messageBus;
            QueueName_OrderSendToPayment = rabbitMqOptions.Value.QueueName_OrderSendToPayment;
        }

        public OrderDetailDto GetOrderById(Guid Id)
        {
            var orders = context.Orders.
                   Include(p => p.OrderLines)
                  .ThenInclude(p => p.Product)
                 .FirstOrDefault(p => p.Id == Id);

            if (orders == null)
                throw new Exception("Order Not Found");
            var result = new OrderDetailDto
            {
                Id = orders.Id,
                Address = orders.Address,
                FirstName = orders.FirstName,
                LastName = orders.LastName,
                PhoneNumber = orders.PhoneNumber,
                UserId = orders.UserId,
                OrderPaid = orders.OrderPaid,
                OrderPlaced = orders.OrderPlaced,
                TotalPrice = orders.TotalPrice,
                PaymentStatus = orders.PaymentStatus,
                OrderLines = orders.OrderLines.Select(ol => new OrderLineDto
                {
                    Id = ol.Id,
                    Name = ol.Product.Name,
                    Price = ol.Product.Price,
                    Quantity = ol.Quantity,

                }).ToList(),

            };
            return result;

        }

        public List<OrderDto> GetOrdersForUser(string UserId)
        {
            var orders = context.Orders.
             Include(p => p.OrderLines)
            .Where(p => p.UserId == UserId)
            .Select(p => new OrderDto
            {
                Id = p.Id,
                OrderPaid = p.OrderPaid,
                OrderPlaced = p.OrderPlaced,
                ItemCount = p.OrderLines.Count(),
                TotalPrice = p.TotalPrice,
                PaymentStatus = p.PaymentStatus
            }).ToList();
            return orders;
        }

    }
}
