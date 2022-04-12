using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyRentalWebService.Models;
using System;

namespace MyRentalWebService.Data.Providers
{
    public class AppDbContext : IdentityDbContext<User,Role,string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
            Database.EnsureCreated();
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public DbSet<RentItem> RentItems { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Option> Options { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ////////////////////////////
            // Change name of Identity tables
            ////////////////////////////          

            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRole");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserToken");

            ////////////////////////////
            // Configure fluent APIs
            ////////////////////////////

            builder.ApplyConfiguration(new UserPropertyConfigure());
            builder.ApplyConfiguration(new RolePropertyConfigure());

            builder.ApplyConfiguration(new ProductPropertyConfigure());
            builder.ApplyConfiguration(new RentPropertyConfigure());
            builder.ApplyConfiguration(new RentItemPropertyConfigure());
            builder.ApplyConfiguration(new CustomerPropertyConfigure());
            builder.ApplyConfiguration(new OptionPropertyConfigure());

            ////////////////////////////
            // Seed admin user and roles
            ////////////////////////////

            //var adminRole = new Role{Name = "Admin", NormalizedName = "Admin".ToLower()};
            //var opRole = new Role{Name = "Operator", NormalizedName = "Operator".ToLower()};
            //var memberRole = new Role{Name = "Member", NormalizedName = "Member".ToLower()};
            //builder.Entity<Role>().HasData(adminRole);
            //builder.Entity<Role>().HasData(opRole);
            //builder.Entity<Role>().HasData(memberRole);

            //var user = new User
            //{
            //    UserName = "aminreza",
            //    PhoneNumber = "09133970043",
            //    Email = "aminreza.c@gmail.com",
            //    IsBlock = false
            //};
            //var _passwordHasher = new PasswordHasher<User>();
            //var hasedPassword = _passwordHasher.HashPassword(user, "123456");
            //user.SecurityStamp = Guid.NewGuid().ToString();
            //user.PasswordHash = hasedPassword;

            //builder.Entity<User>().HasData(user);

            //builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            //{
            //    RoleId = adminRole.Id,
            //    UserId = user.Id
            //});

        }
    }
}