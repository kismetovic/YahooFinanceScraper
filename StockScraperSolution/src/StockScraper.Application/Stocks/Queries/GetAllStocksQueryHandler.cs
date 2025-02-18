﻿using MediatR;
using StockScraper.Application.Common.Interfaces.Persistance;
using StockScraper.Domain.Stocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScraper.Application.Stocks.Queries
{
    public sealed class GetAllStocksQueryHandler : IRequestHandler<GetAllStocksQuery, IEnumerable<StockInfo>>
    {
        private readonly IStockRepository _stocksRepository;
        public GetAllStocksQueryHandler(IStockRepository stocksRepository)
        {
            _stocksRepository = stocksRepository;
        }
        public async Task<IEnumerable<StockInfo>> Handle(GetAllStocksQuery request, CancellationToken cancellationToken)
        {
            var allStocks = await this._stocksRepository.GetAllAsync();

            return allStocks;
        }
    }
}
