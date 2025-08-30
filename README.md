# AvaTrade News API

This project implements the **AvaTrade Developer Home Assignment**.
It is a backend service that periodically fetches trading news from a provider (Polygon.io), enriches it with instrument metadata, stores it, and exposes it via a secured Web API for multiple clients (AvaTradeGo, WebTrader, and marketing tools).

---

## Features

- **News fetcher**: background worker fetches news every X minutes (default: 60).
- **Enrichment**: enriches articles with ticker metadata (mocked for demo, extendable to real ticker/chart enrichment).
- **Storage**: in-memory repository (replaceable with SQLite/SQL for persistence).
- **Web API**: all required routes are present, with subscription as pseudocode
- **Authorization**:
  - Bearer token authentication (mock).
  - Token defined in `appsettings.json` → `Jwt:Key` (hardcoded- for test purpose only).
- **Swagger UI** for testing endpoints.
- **Response caching & CORS** enabled.

---

Swagger is enabled on: `http://localhost:<port>/swagger`
The exact port is printed on startup.(use authorization key from 'Jwt:Key')

## Getting Started

### Prerequisites

- [.NET 9 SDK (9.0.304)](https://dotnet.microsoft.com/download/dotnet/9.0)
- An API key from [Polygon.io](https://polygon.io/) (optional, for real news)

### Configuration

#### Polygon apikey will be provided by request

Edit `appsettings.json`:

```json
"PolygonProvider": {
  "BaseUrl": "https://api.polygon.io",
  "NewsEndpoint": "/v2/reference/news",
  "ApiKey": "YOUR_POLYGON_API_KEY"
}
```
