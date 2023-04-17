using BLZ.Common.Constants;
using BLZ.Common.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLZ.ReportManager.Refit
{
    [Headers("Authorization: Bearer")]
    internal interface IReportingApi
    {
        [Post("/" + Routes.ReportSubmit)]
        Task Submit(Report report);

        [Get("/" + Routes.ReportGetIds)]
        Task<List<int>> GetIds();

        [Get("/" + Routes.ReportGet)]
        Task<List<Report>> GetReports(int id);

        [Post("/" + Routes.ReportMarkAsSpam)]
        Task<List<Report>> MarkAsSpam(string id);

        [Post("/" + Routes.ReportMarkAsSolved)]
        Task<List<Report>> MarkAsSpam(Report report);
    }
}

/*
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
*/

