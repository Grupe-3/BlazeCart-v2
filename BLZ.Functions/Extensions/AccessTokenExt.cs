using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLZ.Functions.Extensions
{
    static internal class AccessTokenExt
    {
        static public ClaimsPrincipal? GetToken(this FunctionContext context)
        {
            if (context.Items.TryGetValue("AccessToken", out object value) && value is ClaimsPrincipal message)
            {
                return message;
            }

            return null;
        }

        static public void AddToken(this FunctionContext context, ClaimsPrincipal principal)
        {
            context.Items.Add("AccessToken", principal);
        }
    }
}
