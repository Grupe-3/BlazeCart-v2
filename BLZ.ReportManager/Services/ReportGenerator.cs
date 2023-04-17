using BLZ.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLZ.ReportManager.Services
{
    internal class ReportGenerator
    {
        static public IEnumerable<Report> CreateReports(int count)
        {
            Random rnd = new Random();
            var reasonArr = Enum.GetValues<Report.ReportReason>();
            for (int i = 0; i < count; i++)
            {
                Report report = new Report();
                report.Created = DateTime.Now;
                report.IsSolved = false;
                report.IsSpam = false;
                report.UserId = "123";
                report.ItemId = rnd.Next(1, 2000);
                report.Reason = (Report.ReportReason)reasonArr.GetValue(rnd.Next(reasonArr.Length))!;
                yield return report;
            }
        }
    }
}
