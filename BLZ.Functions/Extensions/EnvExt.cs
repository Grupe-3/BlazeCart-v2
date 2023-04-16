using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BLZ.Functions.Extensions
{
    static internal class EnvExt
    {
        static public bool IsLocal()
        {
            return string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID"));
        }
    }
}
