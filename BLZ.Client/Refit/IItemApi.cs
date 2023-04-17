using Refit;
using BLZ.Common.Constants;
using BLZ.Client.Models;

namespace BLZ.Client.Refit
{
    [Headers("Authorization: Bearer")]
    public interface IItemApi
    {
        [Get("/" + Routes.ItemsGetCategories)]
        Task<List<string>> GetCategories();

        [Get("/" + Routes.ItemsGetRange)]
        Task<List<Item>> GetRange(int index, int count);

        [Get("/" + Routes.ItemsGetById)]
        Task<Item> GetById(int id);

        [Get("/" + Routes.ItemsGetByCategory)]
        Task<List<Item>> GetByCategory(string category);

        [Get("/" + Routes.ItemsGetByCategoryRange)]
        Task<List<Item>> GetByCategory(string category, int index, int count);

        [Get("/" + Routes.ItemsGetCategoriesIdx)]
        Task<List<string>> GetCategories(int index, int count);

        [Get("/" + Routes.ItemsGetRelated)]
        Task<List<Item>> GetRelated(int id);

        [Get("/" + Routes.ItemsGetRelatedIdx)]
        Task<List<Item>> GetRelated(int id, int index, int count);

        [Get("/" + Routes.ItemsGetCheapestItem)]
        Task<Item> GetCheapest(string name, string category, double price, double amount, int merch, int comparedMerch);
    }
}
