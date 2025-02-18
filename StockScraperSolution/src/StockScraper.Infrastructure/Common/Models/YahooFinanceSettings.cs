using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScraper.Infrastructure.Common.Models
{
    public class YahooFinanceSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string SummaryPath { get; set; } = string.Empty;
        public string ProfilePath { get; set; } = string.Empty;
        public string HistoryPath { get; set; } = string.Empty;
        public Dictionary<string, string> XPaths { get; set; } = new();
    }

}
