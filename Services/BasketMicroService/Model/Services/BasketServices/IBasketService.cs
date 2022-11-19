using BasketMicroService.Model.Dtos;
using BasketMicroService.Model.Services.DiscountServices;
using System;
using System.Threading.Tasks;

namespace BasketMicroService.Model.Services
{
    public interface IBasketService
    {
        BasketDto GetOrCreateBasketForUser(string UserId);
        BasketDto GetBasket(string UserId);
        void AddItemToBasket(AddItemToBasketDto item);
        void RemoveItemFromBasket(Guid ItemId);
        void SetQuantities(Guid itemId, int quantity);
        void TransferBasket(string anonymousId, string UserId);
        void ApplyDiscountToBasket(Guid BasketId, Guid DiscountId);
        ResultDto CheckoutBasket(CheckoutBasketDto checkoutBasket, IDiscountService discountService);
    }
}
