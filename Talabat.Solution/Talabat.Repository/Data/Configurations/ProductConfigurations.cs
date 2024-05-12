using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Repository.Data.Configurations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(P=>P.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(P => P.Description).IsRequired();
            builder.Property(P=>P.PictureUrl).IsRequired();
            builder.Property(P => P.Price).HasColumnType("decimal(18,2)");

            // One Brand Has Many Products
            builder.HasOne(P => P.ProductBrand)
                .WithMany()
                .HasForeignKey(P => P.ProductBrandId);
            // One Type Has Many Products
            builder.HasOne(P => P.ProductType)
                .WithMany()
                .HasForeignKey(P => P.ProductTypeId);
        }
    }
}
