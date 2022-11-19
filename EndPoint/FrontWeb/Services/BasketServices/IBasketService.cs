using Microservices.Web.Frontend.Models.Dtos;
using RestSharp;
using SharedService.Service;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Microservices.Web.Frontend.Services.BasketServices
{
    public interface IBasketService
    {
        BasketDto GetBasket();
        ResultDto AddToBasket(AddToBasketDto addToBasket);
        ResultDto DeleteFromBasket(Guid Id);
        ResultDto UpdateQuantity(Guid BasketItemId, int quantity);
        ResultDto ApplyDiscountToBasket(Guid basketId, Guid discountId);

        ResultDto Checkout(CheckoutDto checkout);
         

    }

    public class BasketService : IBasketService
    {
        private readonly RestClient restClient;
        public string token = "";
        public string UserID = "";
        IHttpContextAccessor htpContextAccessor;

        public BasketService(RestClient restClient, IHttpContextAccessor htpContextAccessor)
        {
            this.restClient = restClient;
            this.htpContextAccessor = htpContextAccessor;
            restClient.Timeout = -1;
            token = htpContextAccessor.HttpContext.Request.Cookies["Auth"];
            UserID = TokenManagerService.GetUserInfo(token).UserID.ToString();
        }

        public ResultDto AddToBasket(AddToBasketDto addToBasket)
        {
            var request = new RestRequest($"/api/Basket?UserId={UserID}", Method.POST);
            request.AddHeader("token", token);
            request.AddHeader("Content-Type", "application/json");
            string serializeModel = JsonSerializer.Serialize(addToBasket);
            request.AddParameter("application/json", serializeModel, ParameterType.RequestBody);
            var response = restClient.Execute(request);
            return GetResponseStatusCode(response);
        }

        private static ResultDto GetResponseStatusCode(IRestResponse response)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return new ResultDto
                {
                    IsSuccess = true,
                };
            }
            else
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = response.ErrorMessage
                };
            }
        }

        public BasketDto GetBasket()
        {
            var request = new RestRequest($"/api/Basket?UserId={UserID}", Method.GET);
            request.AddHeader("token", token);
            IRestResponse response = restClient.Execute(request);
            var basket = JsonSerializer.Deserialize<BasketDto>(response.Content);
            return basket;
        }

        public ResultDto DeleteFromBasket(Guid Id)
        {
            var request = new RestRequest($"/api/Basket/Remove?ItemId={Id}", Method.DELETE);
            request.AddHeader("token", token);
            IRestResponse response = restClient.Execute(request);
            return GetResponseStatusCode(response);
        }

        public ResultDto UpdateQuantity(Guid BasketItemId, int quantity)
        {
            var request = new RestRequest($"/api/Basket?basketItemId={BasketItemId}&quantity={quantity}", Method.PUT);
            request.AddHeader("token", token);
            IRestResponse response = restClient.Execute(request);
            return GetResponseStatusCode(response);
        }

        public ResultDto ApplyDiscountToBasket(Guid basketId, Guid discountId)
        {
       
            var request = new RestRequest($"/api/Basket/{basketId}/{discountId}", Method.PUT);
            request.AddHeader("token", token);
            IRestResponse response = restClient.Execute(request);
            return GetResponseStatusCode(response);

        }

        public ResultDto Checkout(CheckoutDto checkout)
        {
            var request = new RestRequest($"/api/Basket/CheckoutBasket", Method.POST);
            request.AddHeader("token", token);
            request.AddHeader("Content-Type", "application/json");
            string serializeModel = JsonSerializer.Serialize(checkout);
            request.AddParameter("application/json", serializeModel, ParameterType.RequestBody);
            IRestResponse response = restClient.Execute(request);
            return GetResponseStatusCode(response);

        }
    }


    public class AddToBasketDto
    {
        public string BasketId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
    }

}
