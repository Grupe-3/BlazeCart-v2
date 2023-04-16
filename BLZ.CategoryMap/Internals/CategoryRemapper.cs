using BLZ.Common.Extensions;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace BLZ.CategoryMap.Internals;

internal struct CategoryData
{
    public CategoryData()
    {
        CategoryName = "";
        ItemMatcher = new();
    }

    public struct ReplacementData
    {

        [DefaultValue(".*")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Pattern { get; set; }

        [DefaultValue("")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string ReplacementCategory { get; set; }

        public ReplacementData()
        {
            Pattern = ".*";
            ReplacementCategory = "";
        }
    };

    public string CategoryName { get; set; } // Matcher category name

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<ReplacementData> ItemMatcher { get; set; } // List of pairs of <regex pattern, replacement category>
};

internal class CategoryRemapper
{
    private readonly Dictionary<string, CategoryData> _categories = new();

    public CategoryRemapper(IEnumerable<CategoryData> categoryList)
    {
        foreach (var category in categoryList)
        {
            if (category.CategoryName == null || category.CategoryName == "")
            {
                throw new ArgumentException("Category name can't be null");
            }

            /* Append case insensitivity to pattern, if replace cat is empty then set it same as original category name */
            if (category.ItemMatcher.Any())
            {
                category.ItemMatcher.ForEach(item => { item.Pattern += "(?i)"; if (item.ReplacementCategory == "") { item.ReplacementCategory = category.CategoryName; } });
            }
            else
            {
                CategoryData.ReplacementData defaultMatcher = new()
                {
                    Pattern = "(?i).*",
                    ReplacementCategory = category.CategoryName
                };
                category.ItemMatcher.Add(defaultMatcher);
            }
            _categories.Add(category.CategoryName, category);
        }
    }

    /* Returns new item category name */
    public string? TryCategorize(string categoryName, string itemName) => _categories.TryGetValue(categoryName, out CategoryData categoryData)
            ? categoryData.ItemMatcher.Where(i => itemName.ContainsPattern(i.Pattern)).Select(i => i.ReplacementCategory).FirstOrDefault()
            : null;
}
