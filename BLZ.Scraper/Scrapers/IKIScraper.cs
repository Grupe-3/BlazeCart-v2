using BLZ.Common.Extensions;
using BLZ.Common.Models;
using BLZ.Scraper.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Nito.AsyncEx;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using static BLZ.Common.Extensions.CollectionExtensions;

namespace BLZ.Scraper.Scrapers
{

    public class IKIScraper : IScraper
    {
        private const Merchant _merchant = Merchant.IKI;
        private readonly HttpSender<JsonObject> _httpSender;

        private readonly AsyncLazy<JsonArray> _categoryJson;

        private readonly AsyncLazy<IEnumerable<Store>> _stores;
        private readonly AsyncLazy<IEnumerable<Category>> _categories;
        private readonly AsyncLazy<IEnumerable<Item>> _items;

        public IKIScraper(HttpClient httpClient)
        {
            _httpSender = new(httpClient, async (response) =>
            {
                return JsonNode.Parse(await response.Content.ReadAsStreamAsync())!.AsObject();
            });

            _categoryJson = new(async () =>
            {
                var request = new HttpRequestMessage(new HttpMethod("POST"), "https://eparduotuve.iki.lt/api/search/categories")
                {
                    Content = new StringContent("{\"params\":{\"type\":\"categories\",\"show\":true},\"slim\":true}")
                };
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                var resp = await _httpSender.SendAsync(request);
                return resp["data"]!.AsArray();
            });

            _stores = new(GetStoresAsyncImpl);
            _categories = new(GetCategoriesAsyncImpl);
            _items = new(GetItemsAsyncImpl);
        }

        public Merchant GetMerchant()
            => _merchant;

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
            => await _categories;

        public async Task<IEnumerable<Item>> GetItemsAsync()
            => await _items;

        public async Task<IEnumerable<Store>> GetStoresAsync()
            => await _stores;

        private async Task<IEnumerable<Store>> GetStoresAsyncImpl()
            => (await _categoryJson)
                .SelectMany(cat => cat!["storeIds"]!.AsArray())
                .Select(storeId => new Store() { Id = storeId!.ToString(), Merchant = _merchant })
                .GroupBy(store => store.Id)
                .Select(grp =>
                {
                    /* TODO: what is the purpose of this? */
                    var fst = grp.First();
                    return grp.FindFirstOr((s) => s.Name is not null, fst);
                });

        private async Task<IEnumerable<Category>> GetCategoriesAsyncImpl()
        {
            return ParseCategories((await _categoryJson).AsArray());
            static IEnumerable<Category> ParseCategories(JsonArray data, string? parentId = null)
            {
                foreach (var cat in data)
                {
                    var id = cat!["id"]!.ToString();
                    var nameLt = cat["name"]!["lt"]!.ToString();
                    var listing = new Category(nameLt, id, _merchant);
                    listing.ParentCatId = parentId;

                    // Check for categories recursively
                    var subcats = cat["subcategories"]!.AsArray();
                    if (subcats.Any())
                    {
                        foreach (var subcat in ParseCategories(subcats, id))
                        {
                            yield return subcat;
                        }
                    }

                    yield return listing;
                }
            }
        }

        private async Task<IEnumerable<Item>> GetItemsAsyncImpl()
        {
            var categories = await GetCategoriesAsync();
            var query = categories.AsParallel().Select(async i => await GetItemsBy(i.MerchantCatId));
            return (await Task.WhenAll(query)).SelectMany(i => i).Where(i => i.Price != 0).DistinctBy(i => i.MerchantProductId);
            ;
            async Task<IEnumerable<Item>> GetItemsBy(string categoryId)
            {
                var request = new HttpRequestMessage(new HttpMethod("POST"), "https://eparduotuve.iki.lt/api/search/view_products");
                request.Headers.TryAddWithoutValidation("Accept", "application/json, text/plain, */*");
                request.Headers.TryAddWithoutValidation("Host", "eparduotuve.iki.lt");
                request.Content = new StringContent
                (
                    "{\"limit\":1000,\"params\":{\"type\":\"view_products\",\"categoryIds\":[\"" + categoryId + "\"],\"filter\":{}}}"
                );

                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                JsonObject JSONresponse = await _httpSender.SendAsync(request);

                return ParseItemsJSON(JSONresponse, categoryId);
            }
        }

        private static IEnumerable<Item> ParseItemsJSON(JsonObject data, string categoryId)
        {
            return data["data"]!.AsArray()
                .Where(i => IsValidToken(i!.AsObject()))
                .TrySelect<Exception, JsonNode?, Item>(jtoken =>
                {
                    var newItem = new Item(internalID: jtoken!["id"]!.ToString(), jtoken["name"]!["lt"]!.ToString(), categoryId)
                    {
                        Price = (int)(jtoken["prc"]!["p"]?.GetValue<float>() * 100),
                        Image = new Uri(jtoken["photoUrl"]?.ToString()),
                        MeasureUnit = Item.ParseUnitOfMeasurement(jtoken["conversionMeasure"]!.ToString()),
                        Description = jtoken["description"]?["lt"]?.ToString(),
                        DiscountPrice = (int?)(jtoken["prc"]?["s"]?.GetValue<float?>() * 100),
                        LoyaltyPrice = (int?)(jtoken["prc"]?["l"]?.GetValue<decimal?>() * 100),
                        Amount = jtoken["conversionValue"]?.GetValue<float>(),
                        Merchant = _merchant
                    };
                    newItem.FillPerUnitOfMeasureByAmmount();
                    return newItem;
                });
        }

        private static bool IsValidToken(JsonObject jtoken)
        {
            if (isnt_vp(jtoken["prc"],
                jtoken["prc"]!["p"],
                jtoken["conversionValue"],
                jtoken["conversionMeasure"],
                jtoken["name"],
                jtoken["name"]!["lt"],
                jtoken["id"],
                jtoken["description"],
                jtoken["description"]!["lt"],
                jtoken["thumbUrl"],
                jtoken["photoUrl"]
            ))
            {
                return false;
            }

            try
            {
                new Uri(jtoken["thumbUrl"]?.ToString()!);
                new Uri(jtoken["photoUrl"]?.ToString()!);
            }
            catch (UriFormatException) { return false; }

            return true;

            static bool isnt_vp(params JsonNode?[] nodes)
            {
                foreach (var node in nodes)
                {
                    if (isnt_v(node))
                    {
                        return true;
                    }
                }
                return false;
            }

            static bool isnt_v(JsonNode? node)
            {
                if (node == null)
                {
                    return true;
                }

                return node.ToStringNullSafe().IsNullOrEmpty();
            }
        }

        /* TODO: This function is not used anywhere? */
        /*
        private static IEnumerable<Store> ParseStoresJSON(JsonObject data)
        {
            foreach (JsonNode? cat_ in data["chains"]!["stores"]!.AsArray())
            {
                yield return new Store()
                {
                    Id = cat_!["id"]!.ToString(),
                    Name = cat_["name"]!.ToString(),
                    Address = cat_["streetAndBuilding"]!.ToString(),
                    Longitude = cat_["location"]!["geopoint"]!["longitude"]!.ToString(),
                    Latitude = cat_["location"]!["geopoint"]!["latitude"]!.ToString(),
                };
            }
        }
        */
    }
}

