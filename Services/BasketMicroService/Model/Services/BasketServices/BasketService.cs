using AutoMapper;
using BasketMicroService.Infrastructure.Contexts;
using BasketMicroService.MessageingBus;
using BasketMicroService.Model.Dtos;
using BasketMicroService.Model.Entities;
using BasketMicroService.Model.Services.BasketServices.MessageDto;
using BasketMicroService.Model.Services.DiscountServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RabbitMqMessageBus.MessagingBus;
using System;
using System.Linq;

namespace BasketMicroService.Model.Services
{
    public class BasketService : IBasketService
    {
        private readonly BasketDataBaseContext context;
        private readonly IMapper mapper;
        private readonly IMessageBus messageBus;
        private readonly string queueName_CheckoutBasket;
        public BasketService(BasketDataBaseContext context,
            IMapper mapper, IMessageBus messageBus,
            IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            this.context = context;
            this.mapper = mapper;
            this.messageBus = messageBus;
            queueName_CheckoutBasket = rabbitMqOptions.Value.QueueName_BasketCheckout;
        }

        public void AddItemToBasket(AddItemToBasketDto item)
        {
            var basket = context.Baskets.FirstOrDefault(p => p.Id == item.basketId);

            if (basket == null)
                throw new Exception("Basket not founs....!");

            var basketItem = mapper.Map<BasketItem>(item);
            var productDto = mapper.Map<ProductDto>(item);
            createProduct(productDto);
            basket.Items.Add(basketItem);
            context.SaveChanges();
        }


        private ProductDto getProdcut(Guid ProductId)
        {
            var existProduct = context.Products.SingleOrDefault(p => p.ProductId == ProductId);
            if (existProduct != null)
            {
                var product = mapper.Map<ProductDto>(existProduct);
                return product;
            }
            else
                return null;
        }

        private ProductDto createProduct(ProductDto product)
        {
            var existProduct = getProdcut(product.ProductId);
            if (existProduct != null)
            {
                return existProduct;
            }
            else
            {
                var newProduct = mapper.Map<Product>(product);
                context.Add(newProduct);
                context.SaveChanges();
                return mapper.Map<ProductDto>(newProduct);
            }
        }



        public void ApplyDiscountToBasket(Guid BasketId, Guid DiscountId)
        {
            var basket = context.Baskets.Find(BasketId);
            if (basket == null)
                throw new Exception("Basket not found....!");
            basket.DiscountId = DiscountId;
            context.SaveChanges();
        }

        public BasketDto GetBasket(string UserId)
        {
            var basket = context.Baskets
            .Include(p => p.Items)
            .ThenInclude(p => p.Product)
            .SingleOrDefault(p => p.UserId == UserId);

            if (basket == null)
            {
                return null;
            }
            return new BasketDto
            {
                Id = basket.Id,
                UserId = basket.UserId,
                Items = basket.Items.Select(item => new BasketItemDto
                {
                    ProductId = item.ProductId,
                    Id = item.Id,
                    ProductName = item.Product.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.UnitPrice,
                    ImageUrl = item.Product.ImageUrl
                }).ToList(),
            };
        }

        public BasketDto GetOrCreateBasketForUser(string UserId)
        {

            var basket = context.Baskets
                .Include(p => p.Items)
                .ThenInclude(p => p.Product)
                .SingleOrDefault(p => p.UserId == UserId);
            if (basket == null)
            {
                return CreateBasketForUser(UserId);
            }

            return new BasketDto
            {
                Id = basket.Id,
                UserId = basket.UserId,
                DiscountId = basket.DiscountId,
                Items = basket.Items.Select(item => new BasketItemDto
                {
                    ProductId = item.ProductId,
                    Id = item.Id,
                    ProductName = item.Product.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.UnitPrice,
                    ImageUrl = item.Product.ImageUrl,
                }).ToList(),
            };
        }

        public void RemoveItemFromBasket(Guid ItemId)
        {
            var item = context.BasketItems.SingleOrDefault(p => p.Id == ItemId);
            if (item == null)
                throw new Exception("BasketItem Not Found...!");
            context.BasketItems.Remove(item);
            context.SaveChanges();
        }

        public void SetQuantities(Guid itemId, int quantity)
        {
            var item = context.BasketItems.SingleOrDefault(p => p.Id == itemId);
            item.SetQuantity(quantity);
            context.SaveChanges();
        }

        public void TransferBasket(string anonymousId, string UserId)
        {
            var anonymousBasket = context.Baskets
       .Include(p => p.Items)
       .SingleOrDefault(p => p.UserId == anonymousId);

            if (anonymousBasket == null) return;

            var userBasket = context.Baskets.SingleOrDefault(p => p.UserId == UserId);
            if (userBasket == null)
            {
                userBasket = new Basket(UserId);
                context.Baskets.Add(userBasket);
            }
            foreach (var item in anonymousBasket.Items)
            {
                userBasket.Items.Add(new BasketItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                });
            }
            context.Baskets.Remove(anonymousBasket);
            context.SaveChanges();
        }

        private BasketDto CreateBasketForUser(string UserId)
        {
            Basket basket = new Basket(UserId);
            context.Baskets.Add(basket);
            context.SaveChanges();
            return new BasketDto
            {
                UserId = basket.UserId,
                Id = basket.Id,
            };
        }

        public ResultDto CheckoutBasket(CheckoutBasketDto checkoutBasket, IDiscountService discountService)
        {
            // دریافت سبد خرید
            var basket = context.Baskets.Include(p => p.Items)
                .ThenInclude(p => p.Product).SingleOrDefault(p => p.Id == checkoutBasket.BasketId);
            if (basket == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = $"{nameof(basket)} Not Found!",
                };
            }




            // ارسال پیام برای سرویس Order
            BasketCheckoutMessage message =
                mapper.Map<BasketCheckoutMessage>(checkoutBasket);

            int totalPrice = 0;
            foreach (var item in basket.Items)
            {
                var basketitem = new BasketItemMessage
                {
                    BasketItemId = item.Id,
                    ProductId = item.ProductId,
                    Name = item.Product.ProductName,
                    Price = item.Product.UnitPrice,
                    Quantity = item.Quantity,
                };
                totalPrice += item.Product.UnitPrice * item.Quantity;
                message.BasketItems.Add(basketitem);
            }

            // دریافت تخفیف از سرویس discount
            DiscountDto discount = null;
            if (basket.DiscountId.HasValue)
                discount = discountService.GetDiscountById(basket.DiscountId.Value);

            if (discount != null)
            {
                message.TotalPrice = totalPrice - discount.Amount;
            }
            else
            {
                message.TotalPrice = totalPrice;
            }


            //ارسال پیام
            messageBus.SendMessage(message, "", queueName_CheckoutBasket);

            //حذف سبد خرید
            context.Baskets.Remove(basket);
            context.SaveChanges();
            return new ResultDto
            {
                IsSuccess = true,
                Message = "سفارش با موفقیت ثبت شد",
            };
        }
    }
}
