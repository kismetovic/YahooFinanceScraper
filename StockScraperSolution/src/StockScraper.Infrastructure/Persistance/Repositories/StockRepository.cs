using Microsoft.EntityFrameworkCore;
using StockScraper.Application.Common.Interfaces.Persistance;
using StockScraper.Domain.Stocks;

namespace StockScraper.Infrastructure.Persistance.Repositories
{
    public sealed class StockRepository : IStockRepository
    {

        private readonly StockScraperDbContext _context;

        public StockRepository(StockScraperDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StockInfo>> GetAllAsync()
        {
            return await _context.Stocks.OrderByDescending(s => s.DateScraped).ToListAsync();
        }

        public async Task AddAsync(StockInfo stock)
        {
            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();
        }

    }
}
