
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyWebApp.Services;

namespace MyWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // This is where we register the features the app will use.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();                               // adds support for Razor pages like Index.cshtml.cs
            services.AddServerSideBlazor();
            services.AddHttpClient();                               // allow the app to make Http calls
            services.AddControllers();                              // registers support for controllers (like ProductsController.cs)
            services.AddTransient<JsonFileProductService>();        // register my custom service class - transient means a new copy will be created every time it's requested
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();        // show full errors
            }
            else
            {
                app.UseExceptionHandler("/Error");      // show generic error page
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();                          // ensure HTTPS in browsers
            }

            app.UseHttpsRedirection(); // force HTTPS instead of HTTP
            app.UseStaticFiles();      // serve static files like CSS, JS, JSON
            app.UseRouting();          // enables route matching for endpoints
            app.UseAuthorization();    // checks user permissions (not used here)


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();    // handle .cshtml page requests
                endpoints.MapControllers();   // handle API controller routes

                // endpoints.MapGet("/products", (context) => 
                // {
                //     var products = app.ApplicationServices.GetService<JsonFileProductService>().GetProducts();
                //     var json = JsonSerializer.Serialize<IEnumerable<Product>>(products);
                //     return context.Response.WriteAsync(json);
                // });
            });
        }
    }
}