using Microsoft.EntityFrameworkCore;
using BLZ.Common.Models;
using EFCore.BulkExtensions;
using BLZ.DB.Context;

namespace BLZ.DB.Repositories
{

    public class CategoryRepository : ICategoryRepository
    {
        private readonly ScraperDbContext _context;

        public CategoryRepository(ScraperDbContext context)
        {
            _context = context;
        }
        public async Task<List<Category>> GetAllCategoriesAsync()
            => await _context.Categories.ToListAsync();

        public bool IsCategoryActive(string id)
            => _context.Categories.Any(c => c.MerchantCatId == id);

        public async Task<Category?> GetCategoryByIdAsync(string id)
            => await _context.Categories.FirstOrDefaultAsync(c => c.MerchantCatId == id);

        public async Task<ICollection<Item>> GetItemsByCategoryIdAsync(string id)
        {
            var cat = await _context.Categories.FindAsync(id);
            return cat == null ? new List<Item>() : cat.Items;
        }

        public async Task<List<Item>> GetRangeOfItemsByCategoryIdAsync(string id, int index, int count)
            => await _context.Items.Where(i => i.CategoryId == id).Skip(index).Take(count).ToListAsync();

        public async Task<List<Category>> GetCategoriesByNameAsync(string name)
            => await _context.Categories.Where(c => c.NameLT.Contains(name)).ToListAsync();

        public async Task<IEnumerable<Category>> GetRangeOfCategoriesAsync(int index, int count)
            => await _context.Categories.Skip(index).Take(count).ToListAsync();
    }
}