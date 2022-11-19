namespace Microservices.Web.Frontend.Services.ProductServices
{
    public class ProductDto
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public int price { get; set; }
        public ProductCategory productCategory { get; set; }
    }
}
