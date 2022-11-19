using Newtonsoft.Json;
using RestSharp;
using System;

namespace OrderMicroService.Model.Services.ProductServices
{
    public interface IVerifyProductService
    {
        VerifyProductDto Veryfy(ProductDto product);
    }

    public class VerifyProductService : IVerifyProductService
    {
        private readonly RestClient restClient;

        public VerifyProductService(RestClient restClient)
        {
            this.restClient = restClient;
        }

        public VerifyProductDto Veryfy(ProductDto product)
        {
            var request = new RestRequest($"/api/product/verify/{product.ProductId}", Method.GET);
            IRestResponse response = restClient.Execute(request);
            var productOnRemote = JsonConvert.DeserializeObject<ProductVeryfyOnServerProductDto>(response.Content);
            return Verify(product, productOnRemote);

        }

        private VerifyProductDto Verify(ProductDto local, ProductVeryfyOnServerProductDto remote)
        {
            if (local.Name == remote.Name)
            {
                return new VerifyProductDto
                {
                    IsCorrect = true,
                };
            }
            else
            {
                return new VerifyProductDto
                {
                    Name = remote.Name,
                    IsCorrect = false,
                };
            }
        }
    }
    public class VerifyProductDto
    {
        public string Name { get; set; }
        public bool IsCorrect { get; set; }
    }

    public class ProductVeryfyOnServerProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
