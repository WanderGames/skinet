using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context)
    {
        if (!context.Products.Any())
        {
            //specify path this way because of the relative path fro when we publish app
            var productsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");

            var products = JsonSerializer.Deserialize<List<Product>>(productsData);

            if (products == null) return;

            //tracks the enities that have been added
            context.Products.AddRange(products);

            await context.SaveChangesAsync();
        
        }
    }
}
