using BasketMicroService.Filter;
using BasketMicroService.Model.Dtos;
using BasketMicroService.Model.Services;
using BasketMicroService.Model.Services.DiscountServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BasketMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AutenticationFilter]

    public class BasketController : ControllerBase
    {
        private readonly IBasketService basketService;
        public BasketController(IBasketService basketService )
        {
            this.basketService = basketService;
        }

        [HttpGet]
        public IActionResult Get(string UserId)
        {
            var basket = basketService.GetOrCreateBasketForUser(UserId);
            return Ok(basket);
        }

        [HttpPost]
        public IActionResult AddItemToBasket(AddItemToBasketDto request, string UserId)
        {
            var basket = basketService.GetOrCreateBasketForUser(UserId);
            request.basketId = basket.Id;
            basketService.AddItemToBasket(request);
            var basketData = basketService.GetBasket(UserId);
            return Ok();
        }

        [HttpDelete]
        public IActionResult Remove(Guid ItemId)
        {
            basketService.RemoveItemFromBasket(ItemId);
            return Ok();
        }

        [HttpPut]
        public IActionResult SetQuantity(Guid basketItemId, int quantity)
        {
            basketService.SetQuantities(basketItemId, quantity);
            return Ok();
        }

        [HttpPut("{basketId}/{discountId}")]
        public IActionResult ApplyDiscountToBasket(Guid basketId, Guid discountId)
        {
            basketService.ApplyDiscountToBasket(basketId, discountId);
            return Accepted();
        }


        [HttpPost("CheckoutBasket")]
        public IActionResult CheckoutBasket(CheckoutBasketDto checkoutBasket,
            [FromServices] IDiscountService discountService)
        {
            var result = basketService.CheckoutBasket(checkoutBasket, discountService);
            if (result.IsSuccess)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
    }
}
