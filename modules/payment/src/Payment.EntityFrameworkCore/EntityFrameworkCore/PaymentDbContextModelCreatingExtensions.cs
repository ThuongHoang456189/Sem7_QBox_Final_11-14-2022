﻿using Microsoft.EntityFrameworkCore;
using Payment.PaymentRequests;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Payment.EntityFrameworkCore
{
    public static class PaymentDbContextModelCreatingExtensions
    {
        public static void ConfigurePayment(
            this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<PaymentRequest>(b =>
            {
                b.ToTable(PaymentDbProperties.DbTablePrefix + "PaymentRequests", PaymentDbProperties.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.ProductName).IsRequired().HasMaxLength(PaymentRequestConsts.MaxProductNameLength);
                b.Property(x => x.ProductId).IsRequired().HasMaxLength(PaymentRequestConsts.MaxProductIdLength);

                b.HasIndex(x => x.CustomerId);
                b.HasIndex(x => x.State);
            });
        }
    }
}
