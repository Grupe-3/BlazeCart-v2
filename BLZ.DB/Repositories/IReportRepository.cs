using BLZ.Common.Models;

namespace BLZ.DB.Repositories
{
    public interface IReportRepository
    {
        Task SubmitReport(Report report);
        Task<List<int>> GetAvailableReportsAsync();
        Task<List<Report>> GetReportsForItemAsync(int id);
        Task ClearReportsForItemAsync(int id);
    }
}
