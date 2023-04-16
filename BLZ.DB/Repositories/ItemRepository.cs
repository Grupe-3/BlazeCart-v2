using BLZ.DB.Context;
using BLZ.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace BLZ.DB.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly ScraperDbContext _context;
        public ItemRepository(ScraperDbContext context)
        {
            _context = context;
        }

        public async Task<List<Item>> GetRangeOfItemsAsync(int index, int count)
            => await _context.Items.OrderBy(i => i.Id).Skip(index).Take(count).ToListAsync();

        public async Task<Item?> GetItemByIdAsync(int id)
            => await _context.Items.Where(i => i.Id == id).FirstOrDefaultAsync();

        public async Task<List<Item>> GetItemsByNameAsync(string name)
            => await _context.Items.Where(i => i.NameLT == name).ToListAsync();

        public async Task<List<string>> GetItemsCatAsync(int index, int count)
            => await _context.Items.OrderBy(i => i.Id).Skip(index).Take(count).Select(i => i.RemappedCategoryName).ToListAsync();

        public bool IsItemActiveAsync(int id)
            => _context.Items.Any(i => i.Id == id);

        public async Task<List<Item>> GetItemsByCategoryAndMerchantAsync(string category, Merchant? merchant = null)
        {
            var query = _context.Items.Where(i => i.RemappedCategoryName == category);
            if (merchant != null)
            {
                query = query.Where(i => i.Merchant == merchant);
            }
            query = query.Where(i => i.Price != 0 && i.NameLT != null && i.NameLT.Length >= 4);
            return await query.OrderBy(i => i.NameLT).ToListAsync();
        }

        public async Task<List<string>> GetItemCategoriesAsync()
            => await _context.Items.Select(i => i.RemappedCategoryName).Distinct().ToListAsync();

        public async Task<List<Item>> GetItemRelatedAsync(int id, int index = 0, int count = 0)
        {
            var relatedCat = await _context.Items.FindAsync(id);
            if (relatedCat == null)
            {
                return new();
            }

            var query = _context.Items.Where(i => i.RemappedCategoryName == relatedCat.RemappedCategoryName).OrderBy(id => id).Skip(index);
            if (count != 0)
            {
                query = query.Take(count);
            }
            return await query.ToListAsync();
        }
    }
}
