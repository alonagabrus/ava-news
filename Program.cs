using AvaTradeNews.Api.Auth;
using AvaTradeNews.Api.Config;
using AvaTradeNews.Api.Repositories;
using AvaTradeNews.Api.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // 1) Controllers + Swagger
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new() { Title = "AvaTrade news API", Version = "v1", Description = "Api for collect trading news for Avatrade and enabling subscription - Home assignment" });

            // Standard Bearer scheme so the UI fills Authorization header nicely
            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Description = "Type: Bearer <your-token>",
                Name = "Authorization",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });

            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    System.Array.Empty<string>()
                }
            });
        });


        builder.Services.AddCors(o =>
        {
            o.AddDefaultPolicy(p =>
                p.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
        });

        // 3) Caching
        builder.Services.AddMemoryCache();
        builder.Services.AddResponseCaching(); // works if controllers return Cache-Control or [ResponseCache]

        builder.Services
            .AddAuthentication("Bearer")
            .AddScheme<AuthenticationSchemeOptions, MockTokenAuthHandler>("Bearer", _ => { });
        builder.Services.AddHttpClient("Polygon", (sp, http) =>
        {
            var cfg = sp.GetRequiredService<IOptions<PolygonProviderOptions>>().Value;
            http.BaseAddress = new Uri(cfg.BaseUrl!); // relative URIs will resolve against this
        });
        builder.Services.AddAuthorization();

        // 4) Options
        builder.Services.Configure<PolygonProviderOptions>(
            builder.Configuration.GetSection("Providers:Polygon"));

        builder.Services.Configure<NewsFetcherOptions>(
            builder.Configuration.GetSection("NewsFetcher"));


        builder.Services.AddSingleton<INewsRepository, NewsRepository>();
        builder.Services.AddSingleton<IUserRepository, UserRepository>();

        builder.Services.AddSingleton<ISubscriptionRepository, SubscriptionRepository>();
        builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();

        builder.Services.AddScoped<IEnrichmentService, EnrichmentService>();
        builder.Services.AddScoped<INewsQueryService, NewsQueryService>();
        builder.Services.AddHttpClient<INewsExternalProviderService, PolygonNewsService>();
        builder.Services.AddHostedService<NewsFetcherWorker>();

        var app = builder.Build();

        //  Swagger - public for the assignment
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseCors();

        app.UseHttpsRedirection();

        app.UseResponseCaching();

        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
