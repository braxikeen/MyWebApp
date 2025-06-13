// The Product class defines what a 'product' is in our project
// It sets all of its attributes that map to the ones found in 'Products.json' which is found in wwwroot/data
// Therefore, this is the 'Model' part of our project
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyWebApp.Models
{
    public class Product
    {
        public string? Id { get; set; } // getters and setters allow other code to read and write to this variable

        public string? Maker { get; set; }

        // We wanted to name our property 'Image' in C#, but it is called img in the json file
        // so we use JsonPropertyName("x")
        [JsonPropertyName("img")]
        public string? Image { get; set; }

        public string? Url { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public int[]? Ratings { get; set; }

        // we override the default method ToString
        public override string ToString()
        {
            return JsonSerializer.Serialize<Product>(this);
            // we want to serialize the object that we have so that we get a JSON string that describes the object

            // Note:
            // Serialize: transforms object to json
            // Deserialize: transforms json to object
        }
    }

}