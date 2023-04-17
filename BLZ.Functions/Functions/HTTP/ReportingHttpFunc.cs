using BLZ.Common.Constants;
using BLZ.Common.Models;
using BLZ.DB.Repositories;
using BLZ.Functions.Extensions;
using BLZ.Functions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Security.Claims;

namespace BLZ.Functions.Functions.HTTP
{
    public class ReportingHttpFunc
    {
        private readonly IReportRepository _reportRepo;
        public ReportingHttpFunc(IReportRepository reportRepository)
        {
            _reportRepo = reportRepository;
        }

        [Function("HttpReportsSubmit")]
        public async Task<HttpResponseData> ReportSubmit([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Routes.ReportSubmit)] HttpRequestData req, FunctionContext context)
        {
            var report = await req.BodyAs<Report>();
            report.UserId = context.GetToken()!.FindFirstValue("user_id")!;
            await _reportRepo.SubmitReport(report);
            return await req.Ok("Submitted report");
        }

        [Function("HttpReportsGetIds")]
        public async Task<HttpResponseData> ReportGetAvailableIds([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Routes.ReportGetIds)] HttpRequestData req)
            => await req.OkResp(await _reportRepo.GetAvailableReportsAsync());

        [Function("HttpReportsGetReportsById")]
        public async Task<HttpResponseData> ReportGetById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Routes.ReportGet)] HttpRequestData req, int id)
            => await req.OkResp(await _reportRepo.GetReportsForItemAsync(id));

        [Function("HttpReportsMarkUserAsSpam")]
        public async Task<HttpResponseData> ReportMarkUserAsSpam([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Routes.ReportMarkAsSpam)] HttpRequestData req, string id)
        {
            await _reportRepo.MarkAsSpamAsync(id);
            return await req.Ok("Marked user!");
        }

        [Function("HttpReportsMarkAsSolved")]
        public async Task<HttpResponseData> ReportMarkAsSolved([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Routes.ReportMarkAsSolved)] HttpRequestData req)
        {
            var report = await req.BodyAs<Report>();
            await _reportRepo.MarkAsSolvedAsync(report);
            return await req.Ok("Marked as solved!");
        }
    }
}

