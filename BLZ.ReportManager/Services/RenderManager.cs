using BLZ.ReportManager.Render;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLZ.ReportManager.Services
{
    internal class RenderManager
    {
        private readonly IServiceProvider _serviceProvider;
        public RenderManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T Get<T>() where T : IRender
        {
            return _serviceProvider.GetRequiredService<T>();
        }

        public async Task Render<T>() where T : IRender
        {
            await _serviceProvider.GetRequiredService<T>().Render();
        }
    }
}
