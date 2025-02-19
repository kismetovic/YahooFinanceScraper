using StockScraper.Domain.Stocks;

namespace StockScraper.Application.Common.Interfaces.Persistance
{
    public interface IStockRepository
    {
        Task<IEnumerable<StockInfo>> GetAllAsync();
        Task AddAsync(StockInfo stock);
    }
}
