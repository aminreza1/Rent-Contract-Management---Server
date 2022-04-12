using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyRentalWebService.Models
{
    public class RentItem {
        public int Id { get; set; }
        public int RentId { get; set; }
        public Rent Rent { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int PriceWhenRenting { get; set; }
        public RentUnits UnitWhenRenting { get; set; }

    }

    public class RentItemPropertyConfigure : IEntityTypeConfiguration<RentItem>
    {
        public void Configure(EntityTypeBuilder<RentItem> builder)
        {
            builder.ToTable("RentItem");
        }
    }
}
