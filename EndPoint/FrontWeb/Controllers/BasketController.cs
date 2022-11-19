using AuthMicroservice.Filter;
using Microservices.Web.Frontend.Models.Dtos;
using Microservices.Web.Frontend.Services.BasketServices;
using Microservices.Web.Frontend.Services.DiscountServices;
using Microservices.Web.Frontend.Services.ProductServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Frontend.Controllers
{
    [AutenticationFilter]

    public class BasketController : Controller
    {
        private readonly IBasketService basketService;
        private readonly IProductService productService;
        private readonly IDiscountService discountService;
        public string UserId = "";
        public BasketController(IBasketService basketService,
            IProductService productService,
            IDiscountService discountService)
        {
            this.basketService = basketService;
            this.productService = productService;
            this.discountService = discountService;



        }

        public IActionResult Index()
        {
            var LoginInfo = TokenManagerService.GetCurrentUser(Request);
            var UserId = LoginInfo.UserID.ToString();

            var basket = basketService.GetBasket();

            if (basket.discountId.HasValue)
            {
                var discount = discountService.GetDiscountById(basket.discountId.Value);
                basket.DiscountDetail = new DiscountInBasketDto
                {
                    Amount = discount.Data.Amount,
                    DiscountCode = discount.Data.Code,
                };
            }

            return View(basket);
        }

        public IActionResult Delete(Guid Id)
        {
            basketService.DeleteFromBasket(Id);
            return RedirectToAction("Index");
        }

        public IActionResult AddToBasket(Guid ProductId)
        {

            var product = productService.Getproduct(ProductId);
            var basket = basketService.GetBasket();

            AddToBasketDto item = new AddToBasketDto()
            {
                BasketId = basket.id,
                ImageUrl = product.image,
                ProductId = product.id,
                ProductName = product.name,
                Quantity = 1,
                UnitPrice = product.price,
            };
            basketService.AddToBasket(item);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(Guid BasketItemId, int quantity)
        {
            basketService.UpdateQuantity(BasketItemId, quantity);
            return RedirectToAction("Index");

        }


        [HttpPost]
        public IActionResult ApplyDiscount(string DiscountCode)
        {
            if (string.IsNullOrWhiteSpace(DiscountCode))
            {
                return Json(new ResultDto
                {
                    IsSuccess = false,
                    Message = "Please enter the discount code"
                });
            }
            var discount = discountService.GetDiscountByCode(DiscountCode);
            if (discount.IsSuccess == true)
            {
                if (discount.Data.Used)
                {
                    return Json(new ResultDto
                    {
                        IsSuccess = false,
                        Message = "This discount code has already been used"
                    });
                }
                var basket = basketService.GetBasket();
                basketService.ApplyDiscountToBasket(Guid.Parse(basket.id), discount.Data.Id);
                discountService.UseDiscount(discount.Data.Id);
                return Json(new ResultDto
                {
                    IsSuccess = true,
                    Message = "The discount code has been successfully applied to your shopping cart",
                });
            }
            else
            {
                return Json(new ResultDto
                {
                    IsSuccess = false,
                    Message = discount.Message,
                });
            }
        }


        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(CheckoutDto checkout)
        {
            var UserId = TokenManagerService.GetCurrentUser(Request).UserID.ToString();
            checkout.UserId = UserId;
            checkout.BasketId = Guid.Parse(basketService.GetBasket().id);
            var result = basketService.Checkout(checkout);
            if (result.IsSuccess)
                return RedirectToAction("OrderCreated");
            else
            {
                //افزودن پیام
                return View(checkout);
            }
        }

        public IActionResult OrderCreated()
        {
            return View();
        }
    }


}
