using BLZ.Common.Models;

namespace BLZ.Scraper;
public interface IScraper
{
    Merchant GetMerchant();
    Task<IEnumerable<Category>> GetCategoriesAsync();
    Task<IEnumerable<Item>> GetItemsAsync();
    Task<IEnumerable<Store>> GetStoresAsync();
}
