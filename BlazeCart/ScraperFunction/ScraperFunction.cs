﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Scraper;
using DB;
using Models;
using Common;
using CategoryMap;
using CategoryMap.Implementations;

namespace ScraperFunction
{
    public class ScraperFunction
    {
        // TODO: Mapper service, kuris grąžina Map'erį
        // Azure cognitive service
        //
        // CategoryMap
        // Suskaldyti krepšeliai (dvi pigiausios kategorijos)
        // Atra pigiausia parduotuve

        private readonly ScraperDbContext _dbCtx;
        private readonly ICollection<IScraper> _scraperRepo;

        // NOTE: Using static classes means that data is persisted each run
        // thus `StaticCategoryTree.CategoryDict` must be copied
        private IDictionary<string, Category> _categoryDict =
            StaticCategoryTree.GetCategoryDict();

        private IDictionary<string, ICategoryMap> _categoryMappers;

        public ScraperFunction(
            ScraperDbContext dbCtx
            , IScraper[] scraperRepo
            , IDictionary<string, ICategoryMap> categoryMap
        )
        {
            _dbCtx = dbCtx;
            _scraperRepo = scraperRepo;
            //_categoryMappers = categoryMap;
        }

        [FunctionName("ScraperFunction")]
        public async Task Run(
             // Use this tool to check if your
             // crontab expression is correct: https://crontab.cronhub.io
            [TimerTrigger("*/10 * * * *", RunOnStartup = true)]TimerInfo myTimer,
            ILogger log
        )
        {
            log.LogInformation($"`ScraperFunction` Timer trigger function began executing at: {DateTime.UtcNow}");

            try
            {
                // Scraping
                var tasks = new List<Task>();
                foreach (var scraper in _scraperRepo)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        await scraper.Scrape();
                        log.LogInformation(
                            $"ITEM COUNT: {scraper.Items.Count} FROM {scraper.GetType().Name} AT: {DateTime.UtcNow}"
                        );
                    }));
                }
                await Task.WhenAll(tasks);
                log.LogInformation($"Scraping finished at: {DateTime.UtcNow}");

                // Mapping
                foreach (var scraper in _scraperRepo)
                {
                    var mapper = _categoryMappers[scraper.GetType().Name];
                    mapper.Map(scraper.Categories, _categoryDict);
                }

                log.LogInformation($"Category re-mapping finished at: {DateTime.UtcNow}");

                // DB things
                await _dbCtx.Items.ForEachAsync(i => { _dbCtx.Remove(i); });
                await _dbCtx.Categories.ForEachAsync(i => { _dbCtx.Remove(i); });

                var categories = _categoryDict.ToListOfValues();

                categories.GetWithoutChildren().ToList().ForEach(
                    c => _dbCtx.Items.AddRange(c.Items)
                );
                _dbCtx.Categories.AddRange(categories);

                _dbCtx.SaveChanges();
                log.LogInformation($"Scraping finshed. All items updated successfully to DB at: {DateTime.UtcNow}");

            } catch (Exception e) {
                log.LogError("An exception was thrown (aborting): " + e.ToString());
            }
        }
    }
}

