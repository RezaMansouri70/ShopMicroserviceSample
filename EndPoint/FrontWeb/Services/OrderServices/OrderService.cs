using Newtonsoft.Json;
using SharedService.Service;

namespace Microservices.Web.Frontend.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient client;
        private readonly IHttpContextAccessor httpContextAccessor;
        public string token = "";
        public string UserID = "";
        public OrderService(HttpClient restClient, IHttpContextAccessor httpContextAccessor)
        {
            this.client = restClient;
            this.httpContextAccessor = httpContextAccessor;
            token = httpContextAccessor.HttpContext.Request.Cookies["Auth"];
            UserID = TokenManagerService.GetUserInfo(token).UserID.ToString();
        }



        public async Task<List<OrderDto>> GetOrders()
        {

            var response = await client.GetAsync(string.Format("/api/Order?UserId=" + UserID));
            var json = await response.Content.ReadAsStringAsync();
            var orders = JsonConvert.DeserializeObject<List<OrderDto>>(json);
            return orders;
        }

        public async Task<OrderDetailDto> OrderDetail(Guid OrderId)
        {

            var response = await client.GetAsync(string.Format("/api/Order/" + OrderId));
            var json = await response.Content.ReadAsStringAsync();
            var orderdetail = JsonConvert.DeserializeObject<OrderDetailDto>(json);
            return orderdetail;

        }

       
       
    }
}
