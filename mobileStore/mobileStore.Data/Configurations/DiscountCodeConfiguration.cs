using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using mobileStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mobileStore.Data.Enums;

namespace mobileStore.Data.Configurations
{
    public class DiscountCodeConfiguration : IEntityTypeConfiguration<DiscountCode> 
    {
        public void Configure(EntityTypeBuilder<DiscountCode> builder)
        {
            builder.ToTable("DiscountCode");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Code).IsRequired().HasMaxLength(5);

            builder.Property(x => x.Promotion).IsRequired();

            builder.Property(x => x.Count).IsRequired();

            builder.Property(x => x.Describe).IsRequired().HasMaxLength(4000).HasColumnType("nvarchar");

        }
    }
}
