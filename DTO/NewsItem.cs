using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AvaTradeNews.Api.Models;

namespace AvaTradeNews.Api.DTO
{
    public class NewsArticleDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("publisher")]
        public PublisherDto Publisher { get; set; } = new();

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("author")]
        public string Author { get; set; } = string.Empty;

        [JsonPropertyName("published_utc")]
        public DateTime PublishedUtc { get; set; }

        [JsonPropertyName("article_url")]
        public string ArticleUrl { get; set; } = string.Empty;

        [JsonPropertyName("tickers")]
        public List<string> Tickers { get; set; } = new();

        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("keywords")]
        public List<string> Keywords { get; set; } = new();
        public List<InsightDto> Insights { get; set; } = new();
    }


}