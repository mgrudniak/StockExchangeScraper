using System;
using System.Collections.Generic;

namespace StockExchangeScraper.Models
{
    public partial class ExchangeHoliday
    {
        public int ExchangeHolidayId { get; set; }
        public int ExchangeId { get; set; }
        public DateTime Date { get; set; }

        public virtual Exchange Exchange { get; set; }
    }
}
