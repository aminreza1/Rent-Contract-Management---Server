using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace MyRentalWebService.Models
{
    public enum RentUnits
    {
        Hourly,Daily,Weekly,Monthly
    }
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; } 
        public bool IsActive { get; set; }
        public int RentPrice { get; set; } 
        public RentUnits RentUnit { get; set; } 

        public int MinUnitForRent { get; set; }
        public int MaxUnitForRent { get; set; } 

    }
  
    public class ProductPropertyConfigure : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
        }
    }
}