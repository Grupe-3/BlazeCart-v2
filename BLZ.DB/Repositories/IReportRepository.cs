using BLZ.Common.Models;

namespace BLZ.DB.Repositories
{
    public interface IReportRepository
    {
        Task SubmitReport(Report report);
        Task MarkAsSpamAsync(string userId);
        Task MarkAsSolvedAsync(Report report);
        Task<List<int>> GetAvailableReportsAsync();
        Task<List<Report>> GetReportsForItemAsync(int id);
    }
}
