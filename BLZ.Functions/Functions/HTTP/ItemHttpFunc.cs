using BLZ.Common.Constants;
using BLZ.Common.Models;
using BLZ.DB.Repositories;
using BLZ.Functions.Extensions;
using BLZ.Functions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace BLZ.Functions.Functions.HTTP
{
    public class ItemHttpFunc
    {
        private readonly IItemRepository _itemRepository;
        private readonly IAlgorithmService _algoService;
        public ItemHttpFunc(IItemRepository itemRepository, IAlgorithmService algorithmService)
        {
            _itemRepository = itemRepository;
            _algoService = algorithmService;
        }

        [Function("HttpItemsGetCategories")]
        public async Task<HttpResponseData> ItemsGetCategories([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Routes.ItemsGetCategories)] HttpRequestData req)
            => await req.OkResp(await _itemRepository.GetItemCategoriesAsync());

        [Function("HttpItemsGetByCategory")]
        public async Task<HttpResponseData> ItemsGetByCategory([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Routes.ItemsGetByCategory)] HttpRequestData req, string category)
            => await req.OkResp(await _itemRepository.GetItemsByCategoryAndMerchantAsync(category, null));

        [Function("HttpItemsGetByCategoryRange")]
        public async Task<HttpResponseData> ItemsGetByCategoryRange([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Routes.ItemsGetByCategoryRange)] HttpRequestData req, string category, int index, int count)
            => await req.OkResp((await _itemRepository.GetItemsByCategoryAndMerchantAsync(category, null)).Skip(index).Take(count));

        [Function("HttpItemsGetRange")]
        public async Task<HttpResponseData> ItemsGetRange([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Routes.ItemsGetRange)] HttpRequestData req, int index, int count)
            => await req.OkResp(await _itemRepository.GetRangeOfItemsAsync(index, count));

        [Function("HttpItemGetById")]
        public async Task<HttpResponseData> ItemGetById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Routes.ItemsGetById)] HttpRequestData req, int id)
            => await req.OkResp(await _itemRepository.GetItemByIdAsync(id));

        [Function("HttpItemsGetCategoriesIdx")]
        public async Task<HttpResponseData> ItemsGetCategoriesIdx([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Routes.ItemsGetCategoriesIdx)] HttpRequestData req, int index, int count)
            => await req.OkResp(await _itemRepository.GetItemsCatAsync(index, count));

        [Function("HttpItemGetRelated")]
        public async Task<HttpResponseData> ItemGetRelated([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Routes.ItemsGetRelated)] HttpRequestData req, int id)
            => await req.OkResp(await _itemRepository.GetItemRelatedAsync(id));

        [Function("HttpItemsGetRelatedIdx")]
        public async Task<HttpResponseData> ItemsGetRelatedIdx([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Routes.ItemsGetRelatedIdx)] HttpRequestData req, int id, int index, int count)
            => await req.OkResp(await _itemRepository.GetItemRelatedAsync(id, index, count));

        [Function("HttpItemsGetCheapestItem")]
        public async Task<HttpResponseData> ItemsGetCheapest([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Routes.ItemsGetCheapestItem)] HttpRequestData req, string name, string category, double price, double amount, int merch, int comparedMerch)
        {
            /* Joinked from GetCheapestItem - still not quite sure how it works. */
            Merchant? wantedMerch = null;
            if (Enum.IsDefined(typeof(Merchant), merch))
            {
                wantedMerch = (Merchant)merch;
            }
            List<Item> records = await _itemRepository.GetItemsByCategoryAndMerchantAsync(category, wantedMerch);

            Item comparedItem = new()
            {
                Price = (int)(price * 100),
                Amount = (float?)amount,
                Merchant = (Merchant)comparedMerch
            };

            string oldName = comparedItem.NameLT;
            comparedItem.NameLT = _algoService.refactorItemName(oldName).ToLower();
            return await req.OkResp(_algoService.GetCheapestItemAlgorithm(comparedItem, records, oldName));
        }
    }
}

