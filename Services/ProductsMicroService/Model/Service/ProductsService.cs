using Microsoft.EntityFrameworkCore;
using ProductsMicroService.Domain;
using ProductsMicroService.Infrastructure;

namespace ProductsMicroService.Service
{
    public interface IProductService
    {
        List<ProductDto> GetProductList();
        ProductDto GetProduct(Guid Id);
        Guid AddNewProduct(AddNewProductDto addNewProduct);
        bool UpdateProductName(UpdateProductDto updateProduct);
    }

    public class ProductService : IProductService
    {
        private readonly ProductDatabaseContext context;

        public ProductService(ProductDatabaseContext context)
        {
            this.context = context;
        }

        public bool UpdateProductName(UpdateProductDto updateProduct)
        {
            var product = context.Products.Find(updateProduct.ProductId);
            if (product is not null)
            {
                product.Name = updateProduct.Name;
                context.SaveChanges();
                return true;
            }
            else
                return false;
        }

        public Guid AddNewProduct(AddNewProductDto addNewProduct)
        {
            var category = context.Categories.Find(addNewProduct.CategoryId);
            if (category == null)
                throw new Exception("Category Not Found...!");
            Product product = new Product()
            {
                Category = category,
                Description = addNewProduct.Description,
                Image = addNewProduct.Image,
                Name = addNewProduct.Name,
                Price = addNewProduct.Price
            };
            context.Products.Add(product);
            context.SaveChanges();

            return product.Id;
        }

        public ProductDto GetProduct(Guid Id)
        {
            var product = context.Products.Include(p => p.Category)
               .SingleOrDefault(p => p.Id == Id);
            if (product == null)
                throw new Exception("Product Note Found...!");
            ProductDto data = new ProductDto()
            {
                Description = product.Description,
                Id = product.Id,
                Image = product.Image,
                Name = product.Name,
                Price = product.Price,
                productCategory = new ProductCategoryDto
                {
                    Category = product.Category.Name,
                    CategoryId = product.Category.Id
                }
            };
            return data;
        }

        public List<ProductDto> GetProductList()
        {
            var data = context.Products
                 .Include(p => p.Category)
                 .OrderByDescending(p => p.Id)
                 .Select(p => new ProductDto
                 {
                     Description = p.Description,
                     Id = p.Id,
                     Image = p.Image,
                     Name = p.Name,
                     Price = p.Price,
                     productCategory = new ProductCategoryDto
                     {
                         Category = p.Category.Name,
                         CategoryId = p.Category.Id
                     }
                 }).ToList();
            return data;
        }
    }

    public record UpdateProductDto(Guid ProductId, string Name);

    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }
        public ProductCategoryDto productCategory { get; set; }
    }
    public class ProductCategoryDto
    {
        public Guid CategoryId { get; set; }
        public string Category { get; set; }
    }


    public class AddNewProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }
        public Guid CategoryId { get; set; }
    }
}
