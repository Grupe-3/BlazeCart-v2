using BLZ.CategoryMap;
using BLZ.Common.Constants;
using BLZ.DB.Context;
using BLZ.DB.Repositories;
using BLZ.Functions.DesignTimeDB;
using BLZ.Functions.Middleware;
using BLZ.Functions.Services;
using BLZ.Scraper;
using BLZ.Scraper.Scrapers;
using Microsoft.Azure.Functions.Extensions.JwtCustomHandler;
using Microsoft.Azure.Functions.Extensions.JwtCustomHandler.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(worker =>
    {
        worker.UseWhen<AuthMiddleware>((context) =>
        {
            // We want to use this middleware only for http trigger invocations.
            return context.FunctionDefinition.InputBindings.Values
                          .First(a => a.Type.EndsWith("Trigger")).Type == "httpTrigger";
        });
    })
    .ConfigureServices((services) =>
    {
        var conf = new ConfigurationBuilder()
            .SetBasePath (Directory.GetCurrentDirectory())
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        services
            .UseMySqlMig<ScraperDbContext>(conf.GetConnectionString(ConnStrings.SQL)!, ConnStrings.ContextAssembly);
        services.AddStackExchangeRedisCache(o => o.Configuration = conf.GetConnectionString(ConnStrings.Redis));

        services.AddTransient<IItemRepository, ItemRepository>();
        services.AddTransient<ICategoryRepository, CategoryRepository>();
        services.Decorate<IItemRepository, CachedItemRepository>();

        services.AddSingleton<HttpClient>();
        services.AddSingleton<CategoryOrganizer>();
        services.AddSingleton<ScraperService>();
        services.AddSingleton<IAlgorithmService, AlgorithmService>();

        services.AddTransient<IScraper, BarboraScraper>();
        services.AddTransient<IScraper, IKIScraper>();

        services.AddTransient<IFirebaseTokenProvider, CustomTokenProvider>(provider => new CustomTokenProvider(
                issuer: "https://securetoken.google.com/gp3-auth",
                audience: "gp3-auth"));
        services.AddSingleton<ReqAuthService>();

        services.AddResponseCaching();
    })
    .Build();

await host.RunAsync();
