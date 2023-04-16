using BLZ.Common.Models;

namespace BLZ.Common.Extensions
{
    public static class CategoryExtensions
    {
        public static void ForEachR(this IEnumerable<Category> list, Action<Category> act)
        {
            foreach (var cat in list)
            {
                foreach (var subCat in cat.SubCategories)
                {
                    act(subCat);
                }
                act(cat);
            }
        }

        public static IEnumerable<Category> Flatten(this IEnumerable<Category> categories)
        {
            foreach (var cat in categories)
            {
                if (cat.SubCategories.Any())
                {
                    foreach (var child in cat.SubCategories.Flatten())
                    {
                        yield return child;
                    }
                }
                else
                {
                    yield return cat;
                }
            }
        }

        public static string Tree(this IEnumerable<Category> categories)
        {
            return categoryTree(categories, 0);
            static string categoryTree(IEnumerable<Category> categories, int level)
            {
                var str = "";
                foreach (var cat in categories!)
                {
                    str += "\t".Times(level) + cat.ToString() + "\n";
                    str += categoryTree(cat.SubCategories, level + 1);
                }

                return str;
            }
        }

        public static string Tree(this Category category)
        {
            return tree(category, 0);
            static string tree(Category category, int level)
            {
                var str = "\t".Times(level) + category.ToString() + "\n";
                foreach (var sub in category.SubCategories)
                    str += tree(sub, level + 1);

                return str;
            }
        }
    }
}
