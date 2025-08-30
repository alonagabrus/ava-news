using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvaTradeNews.Api.DTO
{
    public class PolygonTickerDto
    {
        public string Ticker { get; set; } = "";
        public string Name { get; set; } = "";
        public string Market { get; set; } = "";
        public decimal? MarketCap { get; set; }
        public string HomepageUrl { get; set; } = "";
        public string Description { get; set; } = "";

    }
}