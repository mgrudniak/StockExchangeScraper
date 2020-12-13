using System;
using System.Collections.Generic;

namespace StockExchangeScraper.Models
{
    public partial class StockQuote
    {
        public int StockQuoteId { get; set; }
        public string CompanyIsin { get; set; }
        public DateTime Date { get; set; }
        public double? Open { get; set; }
        public double? Close { get; set; }
        public double? Min { get; set; }
        public double? Max { get; set; }
        public double? Volume { get; set; }

        public virtual Company Company { get; set; }
    }
}
