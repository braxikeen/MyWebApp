// This file is the 'Service'
// It handles all the reading and writing to the json file (our database)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MyWebApp.Models;

namespace MyWebApp.Services
{
    public class JsonFileProductService
    {
        /* 
            in the constructor, we want the service to get the tools it needs
            in this case, we want our service to know where our files are (where wwwroot is)
            IWebHostEnvironment gives us the WebRootPath */
        public JsonFileProductService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        /*
            we want to be able to use webHostEnvironment from the parameter of the constructor above
            we store it in WebHostEnvironment to use it after constructor is done
            basically storing it, and only giving it a getter because we only set it once */
        public IWebHostEnvironment WebHostEnvironment { get; }

        /*
            now to get the exact location of the json file, we combine the WebRootPath with "/data/products.json"
            this is good practice to avoid any problems if we change the file's location, or send it to someone (so not on our laptop anymore..) */
        private string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "Products.json"); }
        }

        /*
            This method reads the json file, and converts it to a list of products to return it.
            If the json file was empty, or something went wrong, it returns null. 
        */
        public IEnumerable<Product> GetProducts()
        {
            // we use 'using' to ensure file is closed safely after we're done opening it and reading it
            // File.OpenText() opens the file as a text reader
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {
                // we deserialize to convert the JSON to an Object
                // we read the file to the end, and we don't care about it being case-sensitive
                // between the json file and C# code of Product
                // so we are mapping the attributes
                var products = JsonSerializer.Deserialize<Product[]>(jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return (products ?? Array.Empty<Product>()).ToList(); // return empty list if null
            }
        }
        
        /*
            this method adds a rating to a product.
        */
        public void AddRating(string productId, int rating)
        {
            // first, get all of our products
            // therefore, we deserialize the json file here
            var products = GetProducts();

            // if the product that we find (via matching ID) doesn't have any Ratings yet
            if (products.First(x => x.Id == productId).Ratings == null)
            {
                // create a new array with that rating
                products.First(x => x.Id == productId).Ratings = new int[] { rating };
            }

            else
            {
                // convert the array to a list, if the array is somehow null.. just make a new list
                var ratings = products.First(x => x.Id == productId).Ratings?.ToList() ?? new List<int>();
                ratings.Add(rating); // add rating to the list
                products.First(x => x.Id == productId).Ratings = ratings.ToArray(); // convert it back to an array
            }

            // open the json file and delete everything inside (wipe it)
            using var outputStream = File.Open(JsonFileName, FileMode.Truncate);

            // write the entire json file again in a pretty format
            // hence, we serialize our list of products to json
            JsonSerializer.Serialize<IEnumerable<Product>>(
                new Utf8JsonWriter(outputStream, new JsonWriterOptions
                {
                    SkipValidation = true, // skip strict validation because we trust our data
                    Indented = true // format the json with line breaks and spaces for readability
                }),
                products
            );
        }
    }
}