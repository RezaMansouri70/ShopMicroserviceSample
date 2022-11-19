using RestSharp;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Microservices.Web.Frontend.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly RestClient restClient;
        public ProductService(RestClient restClient)
        {
            this.restClient = restClient;
            restClient.Timeout = -1;
         }


        public IEnumerable<ProductDto> GetAllProduct()
        {
            var request = new RestRequest("/api/Product", Method.GET);            
            IRestResponse response = restClient.Execute(request);
            var products = JsonSerializer.Deserialize<List<ProductDto>>(response.Content);
            return products;
        }

        public ProductDto Getproduct(Guid Id)
        {
            var request = new RestRequest($"/api/Product/{Id}", Method.GET);

            IRestResponse response = restClient.Execute(request);

            var product = JsonSerializer.Deserialize<ProductDto>(response.Content);
            return product;
        }
    }
}
