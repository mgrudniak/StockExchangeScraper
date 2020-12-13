using System;
using System.Collections.Generic;

namespace StockExchangeScraper.Models
{
    public partial class Company
    {
        public Company()
        {
            StockQuotes = new HashSet<StockQuote>();
        }

        public string Isin { get; set; }
        public string Name { get; set; }
        public int? SectorId { get; set; }
        public int? IndustryId { get; set; }
        public int? EquityTypeId { get; set; }
        public int? ExchangeId { get; set; }
        public int? EmployeesNumber { get; set; }
        public string Url { get; set; }

        public virtual EquityType EquityType { get; set; }
        public virtual Exchange Exchange { get; set; }
        public virtual Industry Industry { get; set; }
        public virtual Sector Sector { get; set; }
        public virtual ICollection<StockQuote> StockQuotes { get; set; }
    }
}
