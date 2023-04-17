using BLZ.ReportManager.Refit;
using BLZ.ReportManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BLZ.ReportManager.Render
{
    internal class RenderReportOption : IRender
    {
        private readonly InputService _input;
        private readonly IReportingApi _api;
        public RenderReportOption(InputService input, IReportingApi api)
        {
            _input = input;
            _api = api;
        }

        private enum Options {
            GenerateReports, SelectReport
        };

        public async Task Render()
        {
            Random rnd = new Random();
            var option = _input.GetEnum<Options>();
            switch (option)
            {
                case Options.GenerateReports:
                    foreach (var rep in ReportGenerator.CreateReports(10))
                    {
                        await _api.Submit(rep);
                    }
                    break;
                case Options.SelectReport:
                    var availableIds = await _api.GetIds();
                    var id = availableIds[rnd.Next(availableIds.Count)];

                    var reportsForId = await _api.GetReports(id);
                    _input.Log("Id " + id + " has " + reportsForId.Count + " reports.");
                    break;
            }
        }
    }
}
