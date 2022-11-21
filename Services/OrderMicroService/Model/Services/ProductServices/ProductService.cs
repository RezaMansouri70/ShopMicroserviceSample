using OrderMicroService.Infrastructure.Context;
using OrderMicroService.Model.Entities;

namespace OrderMicroService.Model.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly OrderDataBaseContext context;

        public ProductService(OrderDataBaseContext context)
        {
            this.context = context;
        }
        public Product GetProduct(ProductDto productDto)
        {
            var existProduct = context.Products.SingleOrDefault(p => p.ProductId == productDto.ProductId);
            if (existProduct != null)
                return existProduct;
            else
                return CreateNewProduct(productDto);
        }

        private Product CreateNewProduct(ProductDto productDto)
        {
            Product newProduct = new Product()
            {
                ProductId = productDto.ProductId,
                Name = productDto.Name,
                Price = productDto.Price,
            };
            context.Products.Add(newProduct);
            context.SaveChanges();
            return newProduct;
        }

        public bool UpdateProductName(Guid ProductId, string productName)
        {
            var product = context.Products.Find(ProductId);
            if (product is not null)
            {
                product.Name = productName;
                context.SaveChanges();
            }
            return true;
        }

    }
}
