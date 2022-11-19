using ProductsMicroService.Domain;
using ProductsMicroService.DTO;
using ProductsMicroService.Infrastructure;

namespace ProductsMicroService.Service
{
    public interface ICategoryService
    {
        List<CategoryDto> GetCategories();
        Guid AddNewCatrgory(CategoryDto category);
        CategoryDto Getcategory(Guid categoryId);
    }
    public class CategoryService : ICategoryService
    {
        private readonly ProductDatabaseContext context;

        public CategoryService(ProductDatabaseContext context)
        {
            this.context = context;
        }
        public CategoryDto Getcategory(Guid categoryId)
        {
            var category = context.Categories.Find(categoryId);
            return new CategoryDto
            {
                Description = category.Description,
                Id = category.Id,
                Name = category.Name,
            };
        }
        public Guid AddNewCatrgory(CategoryDto category)
        {
            Category newCategory = new Category
            {
                Description = category.Description,
                Name = category.Name,
            };
            context.Categories.Add(newCategory);
            context.SaveChanges();
            return newCategory.Id;
        }

        public List<CategoryDto> GetCategories()
        {
            var data = context.Categories
               .OrderBy(p => p.Name)
               .Select(p => new CategoryDto
               {
                   Description = p.Description,
                   Name = p.Name,
                   Id = p.Id
               }).ToList();
            return data;
        }
    }
}
