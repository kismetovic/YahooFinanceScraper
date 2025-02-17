using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using StockScraper.Domain.Stocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScraper.Infrastructure.Persistance.Configurations
{
    public sealed class StockInfoConfiguration : IEntityTypeConfiguration<StockInfo>
    {
        public void Configure(EntityTypeBuilder<StockInfo> builder)
        {
            builder.ToTable("Stocks");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Ticker)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(s => s.CompanyName)
                .IsRequired()
                .HasMaxLength(255);

            builder.OwnsOne(s => s.MarketCap, mc =>
            {
                mc.Property(m => m.Value).HasColumnName("MarketCapValue").IsRequired();
            });

            builder.Property(s => s.YearFounded).IsRequired();

            builder.Property(s => s.NumberOfEmployees).IsRequired();

            builder.OwnsOne(s => s.Headquarters, hq =>
            {
                hq.Property(h => h.City).HasColumnName("HeadquartersCity").IsRequired().HasMaxLength(100);
                hq.Property(h => h.State).HasColumnName("HeadquartersState").IsRequired().HasMaxLength(50);
            });

            builder.OwnsOne(s => s.Price, p =>
            {
                p.Property(sp => sp.PreviousClose).HasColumnName("PreviousClosePrice").IsRequired();
                p.Property(sp => sp.Open).HasColumnName("OpenPrice").IsRequired();
            });

            builder.Property(s => s.DateRetrieved)
                .IsRequired();
        }
    }
}
