using MediatR;
using StockScraper.Domain.Stocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScraper.Application.Stocks.Queries
{
    public sealed record GetAllStocksQuery() : IRequest<IEnumerable<StockInfo>>;
}
