using OrderMicroService.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderMicroService.Model.Services.ProductServices
{
    public  interface IProductService
    {
        Product GetProduct(ProductDto productDto);
        bool UpdateProductName(Guid ProductId, string productName);
    }


    public class ProductDto
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }
}
