using System;
using System.Collections.Generic;

namespace StockExchangeScraper.Models
{
    public partial class Exchange
    {
        public Exchange()
        {
            Companies = new HashSet<Company>();
            ExchangeHolidays = new HashSet<ExchangeHoliday>();
        }

        public int ExchangeId { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<ExchangeHoliday> ExchangeHolidays { get; set; }
    }
}
