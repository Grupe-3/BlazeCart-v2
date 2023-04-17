using Refit;
using BLZ.Common.Constants;
using BLZ.Client.Models;

namespace BLZ.Client.Refit
{
    [Headers("Authorization: Bearer")]
    public interface ICategoryApi
    {
        [Get("/" + Routes.CategoryGetAll)]
        Task<List<Category>> GetCategories();

        [Get("/" + Routes.CategoryGetById)]
        Task<Category> GetCategoryById(string id);

        [Get("/" + Routes.CategoryGetItemsById)]
        Task<List<Item>> GetItemsById(string id);

        [Get("/" + Routes.CategoryGetByName)]
        Task<Category> GetCategoryByName(string name);

        [Get("/" + Routes.CategoryGetByRange)]
        Task<List<Category>> GetCategoryRange(int index, int count);

        [Get("/" + Routes.CategoryGetByRangeAndId)]
        Task<List<Item>> GetCategoryRangeById(string id, int index, int count);
    }
}
