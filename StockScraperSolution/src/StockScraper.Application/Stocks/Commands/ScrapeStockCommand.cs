﻿using MediatR;
using StockScraper.Domain.Stocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScraper.Application.Stocks.Commands
{
    public sealed record ScrapeStockCommand(List<string> Tickers, DateTime Date) : IRequest<StockInfo>;
}
