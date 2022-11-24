using Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsMicroService.DTO;
using ProductsMicroService.Service;
using System.Data;

namespace ProductsMicroService.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        // GET: api/<CategoryController>
        [HttpGet]
        public IActionResult Get()
        {
            var data = categoryService.GetCategories();
            return Ok(data);

        }

        // POST api/<CategoryController>
        [AutenticationFilter(Roles = "Admin")]
        [HttpPost]
        public IActionResult Post([FromBody] CategoryDto categoryDto)
        {
            categoryService.AddNewCatrgory(categoryDto);
            return Ok();
        }

    }
}
