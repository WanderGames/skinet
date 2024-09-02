using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class CreateProductDTO
{
    //this  tells entity framework that name is a required field
    [Required]
    public string Name { get; set; } = string.Empty;
    //this  tells entity framework that description is a required field
    [Required]
    public string Description { get; set; } = string.Empty;
    
    //this tells entity framework that price must be greater than zero
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
    public decimal Price { get; set; }

    //this  tells entity framework that pictureUrl is a required field
    [Required]
    public string PictureUrl { get; set; } = string.Empty;

    //this  tells entity framework that type is a required field
    [Required]
    public string Type { get; set; } = string.Empty;

    //this  tells entity framework that brand is a required field
    [Required]
    public string Brand { get; set; } = string.Empty;

    //this  tells entity framework that quantityInStock must be at least 1
    [Range(1, int.MaxValue, ErrorMessage = "QuantityInStock must be at least 1")]
    public int QuantityInStock { get; set; }
}
