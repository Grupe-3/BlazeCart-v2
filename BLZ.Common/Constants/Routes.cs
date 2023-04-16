namespace BLZ.Common.Constants
{
    public static class Routes
    {
        /* Category API routes */
        public const string CategoryGetAll = "category/";
        public const string CategoryGetById = "category/{id}";
        public const string CategoryGetItemsById = "category/items/{id}";
        public const string CategoryGetByName = "category/name/{name}/categories";
        public const string CategoryGetByRange = "category/range/{index}/{count}";
        public const string CategoryGetByRangeAndId = "category/rangeId/{id}/{index}/{count}";

        /* Item API routes */
        public const string ItemsGetRange = "items/range/{index}/{count}";

        public const string ItemsGetRelated = "items/related/{id}";
        public const string ItemsGetRelatedIdx = "items/related/{id}/{index}/{count}";

        public const string ItemsGetById = "items/by/{id}";

        public const string ItemsGetCategories = "items/cats/";
        public const string ItemsGetCategoriesIdx = "items/cats/{index}/{count}";

        public const string ItemsGetCheapestItem = "items/cheapest/{name}/{category}/{price}/{amount}/{merch}/{comparedMerch}";

        /* Reporter API routes */
        public const string ReportSubmit = "reports/submit";
        public const string ReportGetIds = "reports/getIds";
        public const string ReportGet = "reports/get/{id}";
        public const string ReportMarkAsSpam = "reports/{id}";
        public const string ReportMarkAsSolved = "reports/solve";
    }
}
