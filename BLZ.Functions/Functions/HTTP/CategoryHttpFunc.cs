using BLZ.Common.Constants;
using BLZ.DB.Repositories;
using BLZ.Functions.Extensions;
using BLZ.Functions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace BLZ.Functions.Functions.HTTP
{
    public class CategoryHttpFunc
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryHttpFunc(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [Function("HttpCategoryGetAll")]
        public async Task<HttpResponseData> GetAllCategories([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Routes.CategoryGetAll)] HttpRequestData req)
            => await req.OkResp(await _categoryRepository.GetAllCategoriesAsync());

        [Function("HttpCategoryGetById")]
        public async Task<HttpResponseData> CategoryGetById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Routes.CategoryGetById)] HttpRequestData req, string id)
            => await req.OkResp(await _categoryRepository.GetCategoryByIdAsync(id));

        [Function("HttpCategoryGetItemsById")]
        public async Task<HttpResponseData> CategoryGetItemsById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Routes.CategoryGetItemsById)] HttpRequestData req, string id)
            => await req.OkResp(await _categoryRepository.GetItemsByCategoryIdAsync(id));

        [Function("HttpCategoryGetByName")]
        public async Task<HttpResponseData> CategoryGetByName([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Routes.CategoryGetByName)] HttpRequestData req, string name)
            => await req.OkResp(await _categoryRepository.GetCategoriesByNameAsync(name));

        [Function("HttpCategoryGetRange")]
        public async Task<HttpResponseData> CategoryGetRange([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Routes.CategoryGetByRange)] HttpRequestData req, int index, int count)
            => await req.OkResp(await _categoryRepository.GetRangeOfCategoriesAsync(index, count));

        [Function("HttpCategoryGetRangeById")]
        public async Task<HttpResponseData> CategoryGetRangeById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Routes.CategoryGetByRangeAndId)] HttpRequestData req, string id, int index, int count)
            => await req.OkResp(await _categoryRepository.GetRangeOfItemsByCategoryIdAsync(id, index, count));
    }
}

