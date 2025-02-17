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
        Task<StockInfo?> GetByTickerAsync(string ticker, DateTime date);
        Task AddAsync(StockInfo stock);
    }
}
