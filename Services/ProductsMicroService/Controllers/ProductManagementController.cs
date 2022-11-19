using Filter;
using Microsoft.AspNetCore.Mvc;
using ProductsMicroService.MessagingBus;
using ProductsMicroService.Service;
using RabbitMqMessageBus.MessagingBus;

namespace ProductsMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AutenticationFilter( Roles ="Admin")]

    public class ProductManagementController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductManagementController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] AddNewProductDto addNewProductDto)
        {
            var productId = productService.AddNewProduct(addNewProductDto);
            return Created($"/api/ProductManagement/get/{productId}", productId);
        }


        [HttpGet]
        public IActionResult Get()
        {
            var products = productService.GetProductList();
            return Ok(products);
        }


        [HttpGet("Id")]
        public IActionResult Get(Guid Id)
        {
            var product = productService.GetProduct(Id);
            return Ok(product);
        }

        [HttpPut]
        public IActionResult Put(UpdateProductDto updateProduct,
           [FromServices] IMessageBus messageBus
           , [FromServices] IConfiguration configuration)
        {
            var result = productService.UpdateProductName(updateProduct);
            if (result)
            {
                UpdateProductNameMessage updateProductNameMessage = new UpdateProductNameMessage
                {
                    Creationtime = DateTime.Now,
                    Id = updateProduct.ProductId,
                    NewName = updateProduct.Name,
                    MessageId = Guid.NewGuid(),
                };

                messageBus.SendMessage(updateProductNameMessage,
                    configuration.GetSection("RabbitMq:ExchangeName_UpdateProductName").Value, "");
            }

            return Ok(result);
        }

    }
}
