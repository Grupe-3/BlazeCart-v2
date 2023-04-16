using BLZ.DB.Context;
using BLZ.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace BLZ.DB.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ScraperDbContext _db;

        public ReportRepository(ScraperDbContext db)
        {
            _db = db;
        }

        public async Task SubmitReport(Report report)
        {
            await _db.Reports.AddAsync(report);
            await _db.SaveChangesAsync();
        }

        public async Task<List<int>> GetAvailableReportsAsync()
            => await _db.Reports.Select(i => i.ItemId).Distinct().ToListAsync();

        public async Task<List<Report>> GetReportsForItemAsync(int id)
            => await _db.Reports.Where(i => i.ItemId == id).ToListAsync();

        public async Task ClearReportsForItemAsync(int id)
            => await _db.Reports.Where(i => i.ItemId == id).ExecuteDeleteAsync();
    }
}
