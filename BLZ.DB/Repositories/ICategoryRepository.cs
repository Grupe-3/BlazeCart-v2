using BLZ.Common.Models;

namespace BLZ.DB.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategoriesAsync();
        bool IsCategoryActive(string id);
        Task<Category?> GetCategoryByIdAsync(string id);
        Task<List<Category>> GetCategoriesByNameAsync(string name);
        Task<ICollection<Item>> GetItemsByCategoryIdAsync(string id);
        Task<IEnumerable<Category>> GetRangeOfCategoriesAsync(int index, int count);
        Task<List<Item>> GetRangeOfItemsByCategoryIdAsync(string id, int index, int count);
    }
}
