using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AvaTradeNews.Api.DTO
{
    public class PublisherDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("homepage_url")]
        public string HomepageUrl { get; set; } = string.Empty;

        [JsonPropertyName("logo_url")]
        public string LogoUrl { get; set; } = string.Empty;

        [JsonPropertyName("favicon_url")]
        public string FaviconUrl { get; set; } = string.Empty;
    }
}