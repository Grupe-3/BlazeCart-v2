using System.Collections.ObjectModel;
using BLZ.Client.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MonkeyCache.FileStore;
using BLZ.Client.Refit;

namespace BLZ.Client.Services;

public class CategoryService
{
    private readonly IItemApi _api;
    public CategoryService(IItemApi api)
    {
        _api = api;
    }

    public async Task<int> GetCategoriesCount()
    {
        var cats = await _api.GetCategories();
        return cats.Count;
    }

    public async Task<ObservableCollection<Category>> GetCategories(int index, int count)
    {
        var cats = await _api.GetCategories(index, count);

        var coll = new ObservableCollection<Category>();
        foreach (var cat in cats)
        {
            coll.Add(new Category(cat));
        }

        return coll;
    }

    public async Task<ObservableCollection<Item>> GetItemsByCategoryId(string id)
    {
        var items = await _api.GetByCategory(id);
        return new(items);
    }

    public async Task<ObservableCollection<Item>> GetRangeOfItemsByCategoryId(string id, int index, int count)
    {
        var items = await _api.GetByCategory(id, index, count);
        return new(items);
    }

}
