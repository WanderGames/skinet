using Core.Entities;

namespace Core.Interfaces;

public interface IProductRepository
{
    //Ireadonly list is just to convey the intention that it will not modify the list
    Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort);
    //returns product or null
    Task<Product?> GetProductByIdAsync(int id);
    Task<IReadOnlyList<string>> GetBrandsAsync();
    Task<IReadOnlyList<string>> GetTypesAsync();
    void AddProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(Product product);
    bool ProductExists(int id);
    Task<bool> SaveChangesAsync();
}
