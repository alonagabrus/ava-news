using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvaTradeNews.Api.Models
{
    public class EnrichedInstrument
    {
        public string Ticker { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Market { get; set; } = string.Empty;
        public decimal? MarketCap { get; set; }
        public string HomepageUrl { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}