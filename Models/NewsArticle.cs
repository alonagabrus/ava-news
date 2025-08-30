using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvaTradeNews.Api.Models
{
    public class NewsArticle : EntityBase
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string ProviderExternalId { get; set; } = string.Empty;
        public string ProviderName { get; set; } = string.Empty;
        public List<string> Instruments { get; set; } = new();
        public List<string> Keywords { get; set; } = new();
        public string PublisherName { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }
        public string ArticleUrl { get; set; } = string.Empty;
        public List<EnrichedInstrument> InstrumentsMeta { get; set; } = new();





    }
}