using Microsoft.EntityFrameworkCore;
using StockScraper.Application.Common.Interfaces.Persistance;
using StockScraper.Domain.Stocks;
using StockScraper.Infrastructure.Persistance;

namespace StockScraper.Infrastructure.Persistance.Repositories
{
    public sealed class StockRepository : IStockRepository
    {

        private readonly StockScraperDbContext _context;

        public StockRepository(StockScraperDbContext context)
        {
            _context = context;
        }

        public async Task<StockInfo?> GetByTickerAsync(string ticker, DateTime date)
        {
            return await _context.Stocks
                .Where(s => s.Ticker == ticker && s.DateRetrieved.Date == date.Date)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(StockInfo stock)
        {
            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();
        }

    }
}
