using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace MyRentalWebService.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsBlock { get; set; }
        public DateTime CreateDateTime { get; set; }
    }

    public class UserPropertyConfigure : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.Property(p=>p.FirstName).HasMaxLength(100);
            builder.Property(p=>p.LastName).HasMaxLength(200);
        }
    }
}
