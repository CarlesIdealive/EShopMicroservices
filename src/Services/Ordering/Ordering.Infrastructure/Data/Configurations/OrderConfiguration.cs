using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Enums;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).HasConversion(orderId => orderId.Value, dbId => OrderId.Of(dbId));

            builder.HasOne<Customer>()
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .IsRequired();

            builder.HasMany(o => o.OrderItems)
                .WithOne()
                .HasForeignKey(oi => oi.OrderId);

            builder.Property(o => o.TotalPrice)
                .HasColumnType("decimal(18,2)");

            builder.ComplexProperty(o => o.OrderName, nameBuilder =>
            {
                nameBuilder.Property(n => n.Value)
                     .IsRequired()
                     .HasMaxLength(100)
                     .IsRequired();
            });

            builder.ComplexProperty(o => o.ShippingAddress, shippingAddressBuilder =>
            {
                shippingAddressBuilder.Property(a => a.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);
                shippingAddressBuilder.Property(a => a.LastName)
                    .IsRequired()
                    .HasMaxLength(50);
                shippingAddressBuilder.Property(a => a.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(200);
                shippingAddressBuilder.Property(a => a.AddressLine)
                    .IsRequired()
                    .HasMaxLength(200);
                shippingAddressBuilder.Property(a => a.Country)
                    .HasMaxLength(50);
                shippingAddressBuilder.Property(a => a.State)
                    .HasMaxLength(50);
                shippingAddressBuilder.Property(a => a.ZipCode)
                    .IsRequired()   
                    .HasMaxLength(5);
            });

            builder.ComplexProperty(o => o.BillingAddress, billingAddressBuilder =>
            {
                billingAddressBuilder.Property(a => a.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);
                billingAddressBuilder.Property(a => a.LastName)
                    .IsRequired()
                    .HasMaxLength(50);
                billingAddressBuilder.Property(a => a.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(200);
                billingAddressBuilder.Property(a => a.AddressLine)
                    .IsRequired()
                    .HasMaxLength(200);
                billingAddressBuilder.Property(a => a.Country)
                    .HasMaxLength(50);
                billingAddressBuilder.Property(a => a.State)
                    .HasMaxLength(50);
                billingAddressBuilder.Property(a => a.ZipCode)
                    .IsRequired()
                    .HasMaxLength(5);
            });

            builder.ComplexProperty(o => o.Payment, paymentBuilder =>
            {
                paymentBuilder.Property(p => p.CardName)
                    .IsRequired()
                    .HasMaxLength(50);
                paymentBuilder.Property(p => p.CardNumber)
                    .IsRequired()
                    .HasMaxLength(24);
                paymentBuilder.Property(p => p.Expiration)
                    .IsRequired()
                    .HasMaxLength(7); // Format MM/YYYY
                paymentBuilder.Property(p => p.Cvv)
                    .IsRequired()
                    .HasMaxLength(3);
                paymentBuilder.Property(p => p.PaymentMethod);
            });

            builder.Property(o => o.Status)
                .HasDefaultValue(OrderStatus.Draft)
                .HasConversion(
                    status => status.ToString(),
                    statusString => Enum.Parse<OrderStatus>(statusString));


        }
    }
}
