using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AvaTradeNews.Api.DTO
{
    public class PolygonResponseDto
    {
        [JsonPropertyName("results")]
        public List<NewsArticleDto> Articles { get; set; } = new();

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

    }
}