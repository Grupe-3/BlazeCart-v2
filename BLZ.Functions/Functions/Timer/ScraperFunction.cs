using BLZ.DB.Context;
using BLZ.Functions.Services;
using BLZ.Scraper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BLZ.Functions.Functions.Timer
{
    public class ScraperFunction
    {
        private readonly ScraperDbContext _dbCtx;
        private readonly IEnumerable<IScraper> _scraperImpls;
        private readonly ScraperService _scraperService;
        private readonly ILogger<ScraperFunction> _logger;

        public ScraperFunction(ScraperDbContext dbCtx, ScraperService scraperService, IEnumerable<IScraper> scraperImpls, ILogger<ScraperFunction> logger)
        {
            _dbCtx = dbCtx;
            _scraperService = scraperService;
            _scraperImpls = scraperImpls;
            _logger = logger;
        }

        [Function("ScraperFunction")]
        public async Task Run([TimerTrigger("0 0 * * *")] TimerInfo tim)
        {
            _logger.LogInformation($"`ScraperFunction` Timer trigger function began executing at: {DateTime.UtcNow}");

            try
            {
                var categoryQueries = (await Task.WhenAll(_scraperImpls.Select(_scraperService.GetCategoriesAsync))).SelectMany(x => x);
                var itemQueries = (await Task.WhenAll(_scraperImpls.Select(_scraperService.GetItemsAsync))).SelectMany(x => x);

                using (var dbContextTransaction = _dbCtx.Database.BeginTransaction())
                {
                    await _dbCtx.Database.ExecuteSqlRawAsync(
                        "SET FOREIGN_KEY_CHECKS = 0;" +
                        "TRUNCATE TABLE " + nameof(_dbCtx.Items) + ";" +
                        "TRUNCATE TABLE " + nameof(_dbCtx.Categories) + ";" +
                        "SET FOREIGN_KEY_CHECKS = 1;"
                   );
                    await _dbCtx.Categories.AddRangeAsync(categoryQueries);
                    await _dbCtx.Items.AddRangeAsync(itemQueries);
                    await _dbCtx.SaveChangesAsync();
                    dbContextTransaction.Commit();
                }
                _logger.LogInformation($"Scraping finshed. All items updated successfully to DB at: {DateTime.UtcNow}");

            }
            catch (Exception e)
            {
                _logger.LogError("An exception was thrown (aborting): " + e.ToString());
            }
        }
    }
}

