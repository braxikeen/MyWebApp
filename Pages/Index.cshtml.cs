// this is the code model behind the Razor page (Index.cshtml)
using System.Collections.Generic;
using MyWebApp.Pages;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MyWebApp.Services;
using MyWebApp.Models;

namespace MyWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        // constructor that injects 2 services: ILogger for logging/debugging and our service class to be used
        public IndexModel(ILogger<IndexModel> logger,
            JsonFileProductService productService)
        {
            _logger = logger;
            ProductService = productService;
        }

        // ProductService stores the service so that the page can use it
        public JsonFileProductService ProductService { get; }

        // the list containing data about the products
        public IEnumerable<Product>? Products { get; private set; }

        // this gets called when someone visits the page
        public void OnGet() => Products = ProductService.GetProducts();
    }
}