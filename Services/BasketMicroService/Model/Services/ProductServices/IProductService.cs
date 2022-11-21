using System;

namespace BasketMicroService.Model.Services.ProductServices
{
    public interface IProductService
    {
        bool UpdateProductName(Guid ProductId, string productName);
    }
}
