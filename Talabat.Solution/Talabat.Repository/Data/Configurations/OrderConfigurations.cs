using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Order_Aggregate;

namespace Talabat.Repository.Data.Configurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // OrderStatue Enum 
            builder.Property(O => O.Status)
                .HasConversion(OStatus => OStatus.ToString() , OStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus) , OStatus));
            // Decimal Warning
            builder.Property(O => O.SubTotal)
                .HasColumnType("decimal(18,2)");
            // Order Own Address => One Table
            builder.OwnsOne(O => O.ShippingAddress, SA => SA.WithOwner());

            // Order Has Many DeliveryMethds
            builder.HasOne(O => O.DeliveryMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
