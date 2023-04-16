using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Microsoft.FSharp.Core;
using Nito.AsyncEx;
using Polly;
using Polly.Wrap;
using ScrapySharp.Extensions;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Net.Http.Json;
using BLZ.Common.Models;
using BLZ.Common.Extensions;
using BLZ.Scraper;
using BLZ.Scraper.Utils;

namespace BLZ.Scraper.Scrapers
{
    public class BarboraScraper : IScraper
    {
        private const Merchant _merchant = Merchant.BARBORA;

        private readonly AsyncPolicyWrap _policy;
        private readonly ILogger<BarboraScraper> _logger;

        private readonly AsyncLazy<IEnumerable<Item>> _items;
        private readonly AsyncLazy<IEnumerable<Category>> _categories;

        private readonly HttpSender<HtmlNode> _httpSender;
        private readonly HttpSender<List<RootCategory>> _categorySender;

        public BarboraScraper(HttpClient httpClient, ILogger<BarboraScraper> logger)
        {
            _logger = logger;
            _httpSender = new HttpSender<HtmlNode>(httpClient,
                res =>
                {
                    var reader_ = new StreamReader(res.Content.ReadAsStream());
                    var itemDoc_ = new HtmlDocument();
                    itemDoc_.LoadHtml(reader_.ReadToEnd());
                    return Task.FromResult(itemDoc_.DocumentNode);
                }
            );

            _categorySender = new(httpClient, async res =>
            {
                return (await res.Content.ReadFromJsonAsync<List<RootCategory>>())!;
            });

            var retryPolicy = Policy
                .Handle<ArgumentNullException>()
                .WaitAndRetryAsync(
                    5,
                    retryAttempt => TimeSpan.FromSeconds(2),
                    onRetry: (exception, sleepDuration, attemptNumber, context) =>
                        _logger.LogInformation($"Failed to get items from {context.First()}; Retrying attempt #{attemptNumber} after {sleepDuration}")
                );

            var fallbackPolicy = Policy
                .Handle<ArgumentNullException>()
                .Or<InvalidOperationException>()
                .FallbackAsync((_) =>
                {
                    _logger.LogInformation("Failed to obtain data.");
                    return Task.CompletedTask;
                });

            _policy = Policy.WrapAsync(retryPolicy, fallbackPolicy);

            _items = new(GetItemsImpl);
            _categories = new(GetCategoriesImpl);
        }

        public Merchant GetMerchant()
            => _merchant;

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
            => await _categories;

        public async Task<IEnumerable<Item>> GetItemsAsync()
            => await _items;

        public Task<IEnumerable<Store>> GetStoresAsync()
            => throw new NotImplementedException();

        internal struct RootCategory
        {
            public RootCategory()
            {
                children = new();
                fullUrl = "";
                name = "";
            }

            public string fullUrl { get; set; }
            public string name { get; set; }
            public List<RootCategory> children { get; set; }
        };

        private async Task<IEnumerable<Category>> GetCategoriesImpl()
        {
            var cats = await _categorySender.SendAsync(new HttpRequestMessage(new HttpMethod("GET"), "https://production-elb.barbora.lt/api/cache/v1/country/LT/categories?languageId=$e8d3e291-bb2e-47e6-a827-a2189b00a441&shopcode="));
            return CreateCategoriesYield(cats).ToList();

            static IEnumerable<Category> CreateCategoriesYield(IEnumerable<RootCategory> cats, string? parentCatId = null)
            {
                foreach (var cat in cats)
                {
                    var url = "https://barbora.lt/" + cat.fullUrl;
                    var parent_cat = new Category
                    (
                        merchantCatId: "/" + cat.fullUrl,
                        nameLT: cat.name,
                        merchant: _merchant
                    )
                    {
                        Uri = new Uri(url)
                    };

                    if (parentCatId != null)
                    {
                        parent_cat.ParentCatId = parentCatId;
                    }

                    var subcats = CreateCategoriesYield(cat.children, parent_cat.MerchantCatId);
                    yield return parent_cat;
                    foreach (var subcat in subcats)
                    {
                        yield return subcat;
                    }
                }
            }
        }
        private async Task<IEnumerable<Item>> GetItemsImpl()
        {
            var categories = await _categories;
            var queries = categories.AsParallel().Select(async cat =>
            {
                return (await GetItemsByCategory(cat)).ToList();
            });

            return (await Task.WhenAll(queries)).SelectMany(i => i).DistinctBy(i => i.MerchantProductId).ToList();
        }
        private async Task<IEnumerable<Item>> GetItemsByCategory(Category category)
        {
            try
            {
                var itemDoc = await _httpSender.SendAsync(
                    new HttpRequestMessage(new HttpMethod("GET"), category.Uri)
                );

                var itemJSON = itemDoc
                    .SelectNodes("/html/body/div[1]/div/div[3]/div/div[3]/div[2]/script")?
                    .First().InnerText
                    .FindFirstRegexMatch("(?<=var products = ).*(?=;)");
                return ProcessItems(itemJSON, category.MerchantCatId).ToList();
            }
            catch (ArgumentNullException)
            {
                _logger.LogInformation($"Barbora probably responded with 500");
                throw;
            }

            static IEnumerable<Item> ProcessItems(string? json, string category_id)
            {
                if (json == null)
                {
                    yield break;
                }

                var json_obj = JsonNode.Parse(json)!.AsArray();
                foreach (var itemJSON in json_obj)
                {
                    yield return new Item(itemJSON!["id"]!.ToString(), itemJSON["title"]!.ToString(), category_id)
                    {
                        Price = (int)(itemJSON["price"]!.GetValue<float>() * 100),
                        Image = new Uri(itemJSON["big_image"]!.ToString()!),
                        MeasureUnit = Item.ParseUnitOfMeasurement(itemJSON["comparative_unit"]!.ToString()),
                        PricePerUnitOfMeasure =
                            (int)(itemJSON["comparative_unit_price"]!.GetValue<float>() * 100),
                        Merchant = _merchant,
                    };
                }
            }
        }
    }
}

