using BLZ.DB.Context;
using BLZ.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace BLZ.DB.Repositories
{
    static internal class ReportExtensions {
        static public IQueryable<Report> WhereValid(this IQueryable<Report> query)
        {
            return query.Where(i => i.IsSolved == false && i.IsSpam == false);
        }
        static public IQueryable<Report> WhereValid(this DbSet<Report> query)
        {
            return query.Where(i => i.IsSolved == false && i.IsSpam == false);
        }
    }


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
            => await _db.Reports.WhereValid().Select(i => i.ItemId).Distinct().ToListAsync();

        public async Task<List<Report>> GetReportsForItemAsync(int id)
            => await _db.Reports.WhereValid().Where(i => i.ItemId == id).ToListAsync();

        public async Task MarkAsSpamAsync(string userId)
        {
            await _db.Reports.Where(i => i.UserId == userId).ExecuteUpdateAsync(i => i.SetProperty(i => i.IsSolved, true).SetProperty(i => i.IsSpam, true));
            await _db.SaveChangesAsync();
        }

        public async Task MarkAsSolvedAsync(Report report)
        {
            report.IsSolved = true;
            _db.Reports.Update(report);
            await _db.SaveChangesAsync();
        }
    }
}
