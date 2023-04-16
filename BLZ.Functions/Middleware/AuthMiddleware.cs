using BLZ.Functions.Extensions;
using BLZ.Functions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace BLZ.Functions.Middleware
{
    internal sealed class AuthMiddleware : IFunctionsWorkerMiddleware
    {
        private readonly ReqAuthService _authService;
        public AuthMiddleware(ReqAuthService authService)
        {
            _authService = authService;
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            var requestData = await context.GetHttpRequestDataAsync();

            if (await _authService.IsAuthorized(requestData!) == false)
            {
                throw new AuthenticationException("Failed to authenticate");
            }

            context.AddToken((await _authService.GetPrincipal(requestData!))!);
            await next(context);
        }
    }
}
