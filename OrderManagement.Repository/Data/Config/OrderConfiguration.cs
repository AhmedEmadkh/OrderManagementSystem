using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Repository.Data.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.OrderDate).HasColumnType("datetime2");

            builder.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");

            builder.Property(o => o.PaymentMethod)
                .HasConversion(
                method => method.ToString(),
                method => Enum.Parse<PaymentMethod>(method,true)
                );

            builder.Property(o => o.OrderStatus)
                .HasConversion(
                state => state.ToString(),
                state => Enum.Parse<OrderStatus>(state,true)
                );

            builder.HasMany(o => o.OrderItems)
                .WithOne(o => o.Order)
                .HasForeignKey(o => o.OrderId);

            builder.HasOne(o => o.Invoice)
                .WithOne(i => i.Order)
                .HasForeignKey<Invoice>(i => i.OrderId);
        }
    }
}
