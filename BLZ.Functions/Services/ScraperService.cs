using BLZ.CategoryMap;
using BLZ.Common.Models;
using BLZ.Scraper;
using BLZ.DB.Repositories;
using Microsoft.Azure.WebJobs.Logging;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLZ.Functions.Services
{
    public class ScraperService
    {
        private readonly CategoryOrganizer _categoryOrganizer = new();

        private readonly Dictionary<IScraper, IEnumerable<Category>> _categories = new();
        private readonly Dictionary<IScraper, IEnumerable<Item>> _items = new();

        public ScraperService(CategoryOrganizer categoryOrganizer)
        {
            _categoryOrganizer = categoryOrganizer;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync(IScraper _scraper)
        {
            if (_categories.TryGetValue(_scraper, out IEnumerable<Category>? value))
            {
                return value;
            }

            return _categories[_scraper] = await _scraper.GetCategoriesAsync();
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(IScraper _scraper)
        {
            if (_items.TryGetValue(_scraper, out IEnumerable<Item>? value))
            {
                return value;
            }

            return _items[_scraper] = await OrganizeItems(_scraper);
        }

        private async Task<IEnumerable<Item>> OrganizeItems(IScraper _scraper)
        {
            var categories = await GetCategoriesAsync(_scraper);
            var cat_map = categories.Select(i => (i.MerchantCatId, i.NameLT)).ToDictionary(i => i.MerchantCatId);
            var items = await _scraper.GetItemsAsync();
            if (items == null)
            {
                return Enumerable.Empty<Item>();
            }

            return CollectRemapped(items).ToBlockingEnumerable();
            async IAsyncEnumerable<Item> CollectRemapped(IEnumerable<Item> items)
            {
                foreach (var item in items)
                {
                    var category_name = cat_map[item.CategoryId].NameLT;
                    var new_cat_name = await _categoryOrganizer.TryRemapAsync(category_name, item.NameLT, _scraper.GetMerchant());
                    if (new_cat_name != null)
                    {
                        item.RemappedCategoryName = new_cat_name;
                        yield return item;
                    }
                }
            }
        }
    }
}
