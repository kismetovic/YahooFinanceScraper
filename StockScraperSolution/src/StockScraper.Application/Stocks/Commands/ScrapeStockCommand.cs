using ErrorOr;
using MediatR;
using StockScraper.Domain.Stocks;

namespace StockScraper.Application.Stocks.Commands
{
    public sealed record ScrapeStockCommand(string Ticker, DateTime Date) : IRequest<ErrorOr<StockInfo>>;
}
