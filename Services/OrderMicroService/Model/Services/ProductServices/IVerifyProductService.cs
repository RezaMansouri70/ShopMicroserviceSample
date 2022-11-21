using System;

namespace OrderMicroService.Model.Services.ProductServices
{
    public interface IVerifyProductService
    {
        VerifyProductDto Veryfy(ProductDto product);
    }
}
