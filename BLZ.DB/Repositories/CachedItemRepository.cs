using BLZ.DB.Extensions;
using BLZ.Common.Models;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLZ.DB.Repositories
{
    public class CachedItemRepository : IItemRepository
    {
        private readonly IItemRepository _itemRepo;
        private readonly IDistributedCache _cache;

        public CachedItemRepository(IItemRepository itemRepo, IDistributedCache cache)
        {
            _itemRepo = itemRepo;
            _cache = cache;
        }

        public Task<Item?> GetItemByIdAsync(int id)
        {
            return _itemRepo.GetItemByIdAsync(id);
        }

        public async Task<List<string>> GetItemCategoriesAsync()
        {
            const string cache_key = "GetItemCategoriesAsync";
            var ret = await _cache.GetValueAsync<List<string>>(cache_key);
            if (ret != null)
            {
                await _cache.RefreshAsync(cache_key);
                return ret;
            }
            else
            {
                var vals = await _itemRepo.GetItemCategoriesAsync();
                await _cache.SetAsync(cache_key, vals);
                return vals;
            }
        }

        public Task<List<Item>> GetItemsByCategoryAndMerchantAsync(string category, Merchant? merchant)
        {
            return _itemRepo.GetItemsByCategoryAndMerchantAsync(category, merchant);
        }

        public Task<List<Item>> GetItemsByNameAsync(string name)
        {
            return _itemRepo.GetItemsByNameAsync(name);
        }

        public Task<List<string>> GetItemsCatAsync(int index, int count)
        {
            return _itemRepo.GetItemsCatAsync(index, count);
        }

        public Task<List<Item>> GetRangeOfItemsAsync(int index, int count)
        {
            return _itemRepo.GetRangeOfItemsAsync(index, count);
        }

        public bool IsItemActiveAsync(int id)
        {
            return _itemRepo.IsItemActiveAsync(id);
        }

        public Task<List<Item>> GetItemRelatedAsync(int id, int index = 0, int count = 0)
        {
            return _itemRepo.GetItemRelatedAsync(id, index, count);
        }
    }
}
