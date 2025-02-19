using MediatR;
using StockScraper.Domain.Stocks;

namespace StockScraper.Application.Stocks.Queries
{
    public sealed record GetAllStocksQuery() : IRequest<IEnumerable<StockInfo>>;
}
