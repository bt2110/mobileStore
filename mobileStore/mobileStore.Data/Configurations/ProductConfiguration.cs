using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using mobileStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mobileStore.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);

            builder.Property(x => x.SeoAlias).IsRequired().HasMaxLength(200);

            builder.Property(x => x.Display).HasMaxLength(500);

            builder.Property(x => x.OperatingSystem).HasMaxLength(500);

            builder.Property(x => x.Camera).HasMaxLength(500);

            builder.Property(x => x.Chip).HasMaxLength(500);

            builder.Property(x => x.ROM_RAM).HasMaxLength(500);

            builder.Property(x => x.Connectivity).HasMaxLength(500);

            builder.Property(x => x.Price).IsRequired();

            builder.Property(x => x.DiscountPercentage).IsRequired();

            builder.Property(x => x.Stock).IsRequired().HasDefaultValue(0);

            builder.Property(x => x.ViewCount).IsRequired().HasDefaultValue(0);

        }
    }
}
