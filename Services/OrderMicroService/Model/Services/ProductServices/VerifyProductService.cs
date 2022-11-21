using Newtonsoft.Json;
using RestSharp;

namespace OrderMicroService.Model.Services.ProductServices
{
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
}
