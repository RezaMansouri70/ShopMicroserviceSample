 using Microsoft.AspNetCore.Mvc;
using ProductsMicroService.Service;

namespace ProductsMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        // GET: api/<ProductController>
        [HttpGet]
        public IActionResult Get()
        {
            var data = productService.GetProductList();
            return Ok(data);
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            Console.WriteLine($"log:  Request {DateTime.Now.ToLongTimeString()}");

            var data = productService.GetProduct(id);
            return Ok(data);
        }


    }

   
}
