using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLZ.ReportManager.Render
{
    internal interface IRender
    {
        Task Render();
    }
}
