﻿using MediatR;
using StockScraper.Application.Common.Interfaces.Persistance;
using StockScraper.Application.Common.Interfaces.Services;
using StockScraper.Domain.Stocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScraper.Application.Stocks.Commands
{
    public sealed class ScrapeStockCommandHandler : IRequestHandler<ScrapeStockCommand, StockInfo>
    {
        private readonly IStockRepository _stocksRepository;
        private readonly IYahooFinanceScraperService _scraperService;

        public ScrapeStockCommandHandler(IStockRepository stocksRepository, IYahooFinanceScraperService scraperService)
        {
            this._stocksRepository = stocksRepository;
            _scraperService = scraperService;
        }

        public async Task<StockInfo> Handle(ScrapeStockCommand request, CancellationToken cancellationToken)
        {
            var existingStock = await _stocksRepository.GetByTickerAsync(request.Tickers.FirstOrDefault()!, request.Date);

            if (existingStock is not null)
                return existingStock;

            var stockData = await _scraperService.ScrapeStockDataAsync(request.Tickers.FirstOrDefault()!, request.Date);

            var stock = new StockInfo(
                Guid.NewGuid(),
                request.Tickers.FirstOrDefault(),
                stockData.CompanyName!,
                stockData.MarketCap!,
                stockData.YearFounded,
                stockData.NumberOfEmployees,
                stockData.Headquarters!,
                stockData.Price!,
                request.Date
            );

            await _stocksRepository.AddAsync(stock);

            return stock;
        }
    }
}
