using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using MyWebApplication.Models;

namespace MyWebApplication.Services
{
      public class JsonFileProductService
    {
        public JsonFileProductService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }

        private string JsonFileName => Path.Combine(WebHostEnvironment.WebRootPath, "data", "products.json");

/*         private string? JsonFileName
        {
            get
            {
                if (WebHostEnvironment?.WebRootPath == null)
                {
                    throw new InvalidOperationException();
                }
                return Path.Combine(WebHostEnvironment.WebRootPath, "data", "products.json");
            }
        } */

         public IEnumerable<Product> GetProducts()
        {
            using var jsonFileReader = File.OpenText(JsonFileName);
            return JsonSerializer.Deserialize<Product[]>(jsonFileReader.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        } 

  /*       public IEnumerable<Product> GetProducts()
        {
            try
            {
                if (!File.Exists(JsonFileName))
                {
                    return Enumerable.Empty<Product>();
                }

            using var jsonFileReader = File.OpenText(JsonFileName);
            var jsonContent = jsonFileReader.ReadToEnd();

            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                return Enumerable.Empty<Product>();
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            var products = JsonSerializer.Deserialize<Product[]>(jsonContent, options);

            return products ?? Enumerable.Empty<Product>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return Enumerable.Empty<Product>();
            }
        } */
                public void AddRating(string productId, int rating)
        {
            var products = GetProducts();

            if (products.First(x => x.Id == productId).Ratings == null)
            {
                products.First(x => x.Id == productId).Ratings = new int[] { rating };
            }
            else
            {
                var ratings = products.First(x => x.Id == productId).Ratings.ToList();
                ratings.Add(rating);
                products.First(x => x.Id == productId).Ratings = ratings.ToArray();
            }

            using var outputStream = File.OpenWrite(JsonFileName);

            JsonSerializer.Serialize<IEnumerable<Product>>(
                new Utf8JsonWriter(outputStream, new JsonWriterOptions
                {
                    SkipValidation = true,
                    Indented = true
                }),
                products
            );
        }
    }
}