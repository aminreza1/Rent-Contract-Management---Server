using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyRentalWebService.Models
{
    public class Option
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class OptionPropertyConfigure : IEntityTypeConfiguration<Option>
    {
        public void Configure(EntityTypeBuilder<Option> builder)
        {
            builder.ToTable("Option");
            builder.Property(p => p.Key).IsRequired().HasMaxLength(20);
            builder.Property(p => p.Key).HasMaxLength(100);


        }
    }
}
