using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Config;

//set up our configuration for our product
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Product> builder)
    {
        //tell entity framework that the price is a decimal and set the precision to 18 and the scale to 2, so 2 decimal places
        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
    }
}
