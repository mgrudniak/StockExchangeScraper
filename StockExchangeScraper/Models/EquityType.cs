using System;
using System.Collections.Generic;

namespace StockExchangeScraper.Models
{
    public partial class EquityType
    {
        public EquityType()
        {
            Companies = new HashSet<Company>();
        }

        public int EquityTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Company> Companies { get; set; }
    }
}
