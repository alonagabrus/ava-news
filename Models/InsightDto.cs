using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvaTradeNews.Api.Models
{
    public class InsightDto
    {
        public string Ticker { get; set; } = string.Empty;
        public string Sentiment { get; set; } = string.Empty;
    }
}