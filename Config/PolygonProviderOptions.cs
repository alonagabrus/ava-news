using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvaTradeNews.Api.Config
{
    public class PolygonProviderOptions
    {
        public string? BaseUrl { get; set; }
        public string? NewsEndpoint { get; set; }
        public string? TickersEndpoint { get; set; }
        public string? ApiKey { get; set; }
        public string? Order { get; set; } = "desc";
        public string? Sort { get; set; } = "published_utc";
        public int Limit { get; set; } = 100;

    }
}