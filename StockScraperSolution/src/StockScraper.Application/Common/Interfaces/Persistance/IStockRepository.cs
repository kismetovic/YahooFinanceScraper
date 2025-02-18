using StockScraper.Domain.Stocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScraper.Application.Common.Interfaces.Persistance
{
    public interface IStockRepository
    {
        Task<IEnumerable<StockInfo>> GetAllAsync();
        Task AddAsync(StockInfo stock);
    }
}
