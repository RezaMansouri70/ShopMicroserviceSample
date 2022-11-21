namespace Microservices.Web.Frontend.Services.BasketServices
{
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
