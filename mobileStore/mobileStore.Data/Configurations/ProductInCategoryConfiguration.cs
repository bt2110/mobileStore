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
    public class ProductInCategoryConfiguration : IEntityTypeConfiguration<ProductInCategory>
    {
        public void Configure(EntityTypeBuilder<ProductInCategory> builder)
        {
            builder.ToTable("ProductInCategories");

            builder.HasKey(x => new { x.CategoryId, x.ProductId });

            builder.HasOne(x => x.Product).WithMany(x => x.ProductInCategories)
                .HasForeignKey(pc => pc.ProductId);

            builder.HasOne(x => x.Category).WithMany(x => x.ProductInCategories)
              .HasForeignKey(x => x.CategoryId);
        }
    }
}
