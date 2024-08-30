using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

//The [ApiController] and [Route("api/[controller]")] attributes are used to configure the controller's behavior and routing
[ApiController]
[Route("api/[controller]")]
//controllerbase allows us to use http methods without having to use views, also use our db context we made called storecontext
public class ProductsController(StoreContext context) : ControllerBase
{
    /// <summary>
    /// Get all Products
    /// </summary>
    /// <returns>A list of Products</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return await context.Products.ToListAsync();
    }

    /// <summary>
    /// Get a single Product by Id
    /// </summary>
    /// <param name="id">The id of the Product to retrieve</param>
    /// <returns>The Product with the matching id, or NotFound if not found</returns>
    [HttpGet("{id:int}")]//api/products/1 where 1 is the id
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await context.Products.FindAsync(id);

        if (product == null) return NotFound();

        return product;
    }

    /// <summary>
    /// Create a new Product
    /// </summary>
    /// <param name="product">The new Product to create</param>
    /// <returns>The newly created Product, or BadRequest if the product is invalid</returns>
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        context.Products.Add(product);
        await context.SaveChangesAsync();

        return product;
    }

    /// <summary>
    /// Update a Product
    /// </summary>
    /// <param name="id">The id of the Product to update</param>
    /// <param name="product">The Product to update</param>
    /// <returns>NoContent if the product is updated, or BadRequest if the product is invalid or the id in the url doesn't match the id in the body</returns>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        //if the id in the url doesn't match the id in the body, return bad request
        if (product.Id != id || !ProductExists(id)) return BadRequest("Cannot update this product");

        //tell entity framework that the product is being modified
        context.Entry(product).State = EntityState.Modified;

        await context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Delete a Product
    /// </summary>
    /// <param name="id">The id of the Product to delete</param>
    /// <returns>NoContent if the product is deleted, or NotFound if the product is not found</returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await context.Products.FindAsync(id);

        if (product == null) return NotFound();

        //tell entity framework to track the product as deleted
        context.Products.Remove(product);

        await context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Check if a product with the given id exists in the database
    /// </summary>
    /// <param name="id">The id of the product to check</param>
    /// <returns>True if the product exists, false otherwise</returns>
    private bool ProductExists(int id)
    {
        return context.Products.Any(x => x.Id == id);
    }
}
