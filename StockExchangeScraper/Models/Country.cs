using System;
using System.Collections.Generic;

namespace StockExchangeScraper.Models
{
    public partial class Country
    {
        public Country()
        {
            Exchanges = new HashSet<Exchange>();
        }

        public int CountryId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Exchange> Exchanges { get; set; }
    }
}
