using System.Collections.Generic;
using MyWebApp.Models;
using MyWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace MyWebApp.Controllers
{
    [ApiController] // this declares that it is the Controller class, which will handle web API requests
    [Route("[controller]")] // this sets the route to be /products - this is done by removing the word controller from the class name

    // controller class inherits from class ControllerBase
    public class ProductsController : ControllerBase
    {
        // dependency injection so that the controller can use it to update and load products
        public ProductsController(JsonFileProductService productService) =>
            ProductService = productService;

        // stores the injected service to be able to use it
        public JsonFileProductService ProductService { get; }

        // this deals with a GET request sent to /products
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            //calls GetProducts that we defined in the service that returns the list of products
            // since we're in the controller, ASP.NET and ControllerBase takes away the work for us to serialize
            // it does it by itself
            return ProductService.GetProducts();
        }

        [Route("Rate")] // this sets the route to be /products/rate
        [HttpGet] // responds to a GET request for this route

        // an ActionResult returns an Ok() or an error later..
        public ActionResult Get([FromQuery] string productId,   // products/rate?productId=abc
                                [FromQuery] int rating)         // products/rate?productId=abc&rating=x     this is the final route
        {
            ProductService.AddRating(productId, rating);
            return Ok();
        }
    }
}