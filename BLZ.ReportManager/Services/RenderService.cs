using BLZ.ReportManager.Render;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLZ.ReportManager.Services
{
    internal class RenderService
    {
        private readonly ILogger<RenderService> _logger;
        private readonly RenderManager _renderer;
        public RenderService(RenderManager renderer, ILogger<RenderService> logger)
        {
            _renderer = renderer;
            _logger = logger;
        }

        public async Task Run()
        {
            await _renderer.Render<RenderAuth>();

            await _renderer.Render<RenderReportOption>();

            while (true)
            {


                await Task.Delay(1000);
            }
        }
    }
}
