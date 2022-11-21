using OrderMicroService.Infrastructure.Context;
using OrderMicroService.MessagingBus;
using OrderMicroService.Model.Entities;
using OrderMicroService.Model.Services.ProductServices;

namespace OrderMicroService.Model.Services.RegisterOrderServices
{
    public class RegisterOrderService : IRegisterOrderService
    {

        private readonly OrderDataBaseContext context;
        private readonly IProductService productService;

        public RegisterOrderService(OrderDataBaseContext context, IProductService productService)
        {
            this.context = context;
            this.productService = productService;
        }

        public bool Execute(BasketDto basket)
        {
            List<OrderLine> orderLines = new List<OrderLine>();
            foreach (var basketItem in basket.BasketItems)
            {
                var product = productService.GetProduct(new ProductDto
                {
                    Name = basketItem.Name,
                    Price = basketItem.Price,
                    ProductId = basketItem.ProductId,
                });

                orderLines.Add(new OrderLine
                {
                    Id = Guid.NewGuid(),
                    Quantity = basketItem.Quantity,
                    Product = product,
                });

            }

            Order order = new Order(basket.UserId,
                  basket.FirstName,
                  basket.LastName,
                  basket.Address,
                  basket.PhoneNumber,
                  basket.TotalPrice,
                  orderLines);

            context.Orders.Add(order);
            context.SaveChanges();
            return true;
        }
    }
}
