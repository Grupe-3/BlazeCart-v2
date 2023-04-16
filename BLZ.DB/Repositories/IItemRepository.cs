using BLZ.Common.Models;

namespace BLZ.DB.Repositories
{
    public interface IItemRepository
    {
        Task<List<Item>> GetRangeOfItemsAsync(int index, int count);
        Task<Item?> GetItemByIdAsync(int id);
        Task<List<Item>> GetItemsByNameAsync(string name);
        bool IsItemActiveAsync(int id);
        Task<List<string>> GetItemsCatAsync(int index, int count);
        Task<List<Item>> GetItemsByCategoryAndMerchantAsync(string category, Merchant? merchant);
        Task<List<string>> GetItemCategoriesAsync();
        Task<List<Item>> GetItemRelatedAsync(int id, int index = 0, int count = 0);
    }
}
