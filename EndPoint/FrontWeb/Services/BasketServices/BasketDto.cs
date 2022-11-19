using System;
using System.Collections.Generic;
using System.Linq;

namespace Microservices.Web.Frontend.Services.BasketServices
{
    public class BasketDto
    {
        public string id { get; set; }
        public string userId { get; set; }
        public Guid? discountId { get; set; }
        public DiscountInBasketDto DiscountDetail { get; set; } = null;

        public List<BasketItem> items { get; set; }
        public int TotalPrice()
        {
            int result = items.Sum(p => p.unitPrice * p.quantity);
            if (discountId.HasValue)
                result = result - DiscountDetail.Amount;
            return result;
        }
    }
    public class DiscountInBasketDto
    {
        public int Amount { get; set; }
        public string DiscountCode { get; set; }
    }

}
