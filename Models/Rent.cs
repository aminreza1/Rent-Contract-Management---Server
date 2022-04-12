using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace MyRentalWebService.Models
{
    public enum CalculationMethod
    {
        Specified = 0, Update = 1, Fixed = 2
    }
    public class Rent
    {
        public Rent()
        {
            Items = new List<RentItem>();
        }
        public int Id { get; set; }
        public CalculationMethod CalculationMethod { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateTime RentDateTime { get; set; }
        public DateTime? ReturnDateTime { get; set; }
        public int? FinalCost { get; set; }
        public int? PredictedCost { get; set; }
        public bool IsTerminated { get; set; }
        public string Description { get; set; }
        public IEnumerable<RentItem> Items { get; set; }

    }

    public class RentPropertyConfigure : IEntityTypeConfiguration<Rent>
    {
        public void Configure(EntityTypeBuilder<Rent> builder)
        {
            builder.ToTable("Rent");
            builder.Property(p => p.Description).HasMaxLength(1000);
        }
    }
}
