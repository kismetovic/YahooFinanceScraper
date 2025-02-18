using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScraper.Contracts.Stocks
{
    public sealed record GetAllStocksResponse(IEnumerable<StockResponse> responses);
}
