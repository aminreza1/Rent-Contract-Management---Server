using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace MyRentalWebService.Models
{
    public class Customer
    {
        public Customer()
        {
            Rents = new List<Rent>();
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public IEnumerable<Rent> Rents { get; set; }

    }

    public class CustomerPropertyConfigure : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer");
            builder.Property(p => p.FirstName).HasMaxLength(100);
            builder.Property(p => p.LastName).HasMaxLength(200).IsRequired();
            builder.Property(p => p.NationalCode).HasMaxLength(15);
            builder.Property(p => p.Phone).HasMaxLength(15);
            builder.Property(p => p.Mobile).HasMaxLength(15).IsRequired();
            builder.Property(p => p.Address).HasMaxLength(1000);
        }
    }
}
