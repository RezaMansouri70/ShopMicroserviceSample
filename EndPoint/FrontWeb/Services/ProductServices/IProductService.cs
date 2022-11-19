using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Frontend.Services.ProductServices
{
    public interface IProductService
    {
        IEnumerable<ProductDto> GetAllProduct();
        ProductDto Getproduct(Guid Id);
    }
}
