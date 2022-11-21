using ProductsMicroService.DTO;

namespace ProductsMicroService.Service
{
    public interface ICategoryService
    {
        List<CategoryDto> GetCategories();
        Guid AddNewCatrgory(CategoryDto category);
        CategoryDto Getcategory(Guid categoryId);
    }
}
