using Microsoft.EntityFrameworkCore;
using StockScraper.Domain.Stocks;
using StockScraper.Infrastructure.Persistance.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace StockScraper.Infrastructure.Persistance
{
    public sealed class StockScraperDbContext : DbContext
    {
        public DbSet<StockInfo> Stocks { get; set; }

        public StockScraperDbContext(DbContextOptions<StockScraperDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StockInfoConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
