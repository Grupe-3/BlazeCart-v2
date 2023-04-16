using BLZ.CategoryMap.Internals;
using BLZ.Common.Models;
using Nito.AsyncEx;
using System.Reflection.Metadata.Ecma335;

namespace BLZ.CategoryMap;
public class CategoryOrganizer
{
    private readonly AsyncLazy<CategoryRemapper> _barboraRemapper;
    private readonly AsyncLazy<CategoryRemapper> _ikiRemapper;
    private readonly AsyncLazy<HashSet<string>> _categories;

    public CategoryOrganizer()
    {
        _barboraRemapper = new(async () =>
        {
            var map = await ResourceLoader.ReadResourceJsonAsync<List<CategoryData>>("BarboraMap.json");
            return map == null ? throw new ArgumentException("Invalid barbora map json") : new CategoryRemapper(map);
        });

        _ikiRemapper = new(async () =>
        {
            var map = await ResourceLoader.ReadResourceJsonAsync<List<CategoryData>>("IkiMap.json");
            return map == null ? throw new ArgumentException("Invalid iki map json") : new CategoryRemapper(map);
        });

        _categories = new(async () =>
        {
            var categories = await ResourceLoader.ReadResourceJsonAsync<List<string>>("Categories.json");
            return categories == null ? throw new ArgumentException("Invalid categories json") : new HashSet<string>(categories);
        });
    }

    public async Task<string?> TryRemapAsync(string categoryName, string itemName, Merchant merchant)
    {
        try
        {
            var cat = merchant switch
            {
                Merchant.IKI => (await _ikiRemapper).TryCategorize(categoryName, itemName),
                Merchant.BARBORA => (await _barboraRemapper).TryCategorize(categoryName, itemName),
                _ => null
            };
            if (cat == null || cat == "" || (await _categories).Contains(cat) == false)
            {
                return null;
            }
            return cat;
        }
        catch (ArgumentException)
        {
            return null;
        }
    }
}
