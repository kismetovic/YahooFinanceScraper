using ErrorOr;
using MediatR;
using StockScraper.Application.Common.Interfaces.Persistance;
using StockScraper.Application.Common.Interfaces.Services;
using StockScraper.Domain.Stocks;

namespace StockScraper.Application.Stocks.Commands
{
    public sealed class ScrapeStockCommandHandler : IRequestHandler<ScrapeStockCommand, ErrorOr<StockInfo>>
    {
        private readonly IStockRepository _stocksRepository;
        private readonly IYahooFinanceScraperService _scraperService;

        public ScrapeStockCommandHandler(IStockRepository stocksRepository, IYahooFinanceScraperService scraperService)
        {
            this._stocksRepository = stocksRepository;
            _scraperService = scraperService;
        }

        public async Task<ErrorOr<StockInfo>> Handle(ScrapeStockCommand request, CancellationToken cancellationToken)
        {

            var stockData = _scraperService.ScrapeStockDataAsync(request.Ticker, request.Date);

            if (stockData.IsError)
                return stockData.Errors;

            await _stocksRepository.AddAsync(stockData.Value);

            return stockData.Value;
        }
    }
}
