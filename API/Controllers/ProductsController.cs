using Azure.Core.Pipeline;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

//The [ApiController] and [Route("api/[controller]")] attributes are used to configure the controller's behavior and routing
[ApiController]
[Route("api/[controller]")]
//controllerbase allows us to use http methods without having to use views, also use our db context we made called storecontext
public class ProductsController(IGenericRepository<Product> productRepository) : ControllerBase
{

    /// <summary>
    /// Get a list of Products, filtered by brand, type, and sorted by sort
    /// </summary>
    /// <param name="brand">The brand to filter by</param>
    /// <param name="type">The type to filter by</param>
    /// <param name="sort">The sort order, for example "priceAsc"</param>
    /// <returns>A list of Products</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? brand, string? type, string? sort)
    {
        var spec = new ProductSpecification(brand, type, sort);
        var products = await productRepository.ListAsync(spec);
        
        return Ok(products);
    }

    /// <summary>
    /// Get a single Product by Id
    /// </summary>
    /// <param name="id">The id of the Product to retrieve</param>
    /// <returns>The Product with the matching id, or NotFound if not found</returns>
    [HttpGet("{id:int}")]//api/products/1 where 1 is the id
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await productRepository.GetbyIdAsync(id);

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
        productRepository.Add(product);

        if (await productRepository.SaveAllAsync())
        {
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        return BadRequest("Problem Creating product");
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

        productRepository.Update(product);

        if (await productRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Problem updating product()");
    }

    /// <summary>
    /// Delete a Product
    /// </summary>
    /// <param name="id">The id of the Product to delete</param>
    /// <returns>NoContent if the product is deleted, or NotFound if the product is not found</returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await productRepository.GetbyIdAsync(id);

        if (product == null) return NotFound();

        //tell entity framework to track the product as deleted
        productRepository.Remove(product);

        if (await productRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Problem deleting product()");
    }

    /// <summary>
    /// Get all the brands
    /// </summary>
    /// <returns>A list of all the brands</returns>
    [HttpGet("Brands")] //api/products/Brands
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var spec = new BrandListSpecification();

        return Ok(await productRepository.ListAsync(spec));
    }

    /// <summary>
    /// Get all the types
    /// </summary>
    /// <returns>A list of all the types</returns>
    [HttpGet("Types")] //api/products/Types
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var spec = new TypeListSpecification();

        return Ok(await productRepository.ListAsync(spec));
    }


    /// <summary>
    /// Check if a product with the given id exists in the database
    /// </summary>
    /// <param name="id">The id of the product to check</param>
    /// <returns>True if the product exists, false otherwise</returns>
    private bool ProductExists(int id)
    {
        return productRepository.Exists(id);
    }
}
