using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using MyWebApplication.Models;
using MyWebApplication.Services;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Controllers
{
    [Route("[controller]")] //route goes here
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public ProductsController(JsonFileProductService productService)
        {
            ProductService = productService;
        }
        public JsonFileProductService ProductService { get; }

        [HttpGet]
        public IEnumerable<Product> Get() => ProductService.GetProducts();

       /*  [Route("Rate")]
        [HttpGet]
        public ActionResult Get(
            [FromQuery] string ProductId, //this comes from a user query 
            [FromQuery] int Rating) 
        {
            ProductService.AddRating(ProductId, Rating);
            return Ok();
        } */
        [HttpPatch]
        public ActionResult Patch([FromBody] RatingRequest request)
        {
            if (request?.ProductId == null) return BadRequest();

            ProductService.AddRating(request.ProductId, request.Rating);

            return Ok();
        }

        public class RatingRequest
        {
            public string? ProductId { get; set; }
            public int Rating { get; set; }
        }
        
    }
}