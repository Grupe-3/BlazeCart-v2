using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using System.Net;
using System.Security.Authentication;

namespace BLZ.Functions.Middleware
{
    internal sealed class ExceptionMiddleware : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (AuthenticationException)
            {
                var httpReqData = await context.GetHttpRequestDataAsync();
                if (httpReqData == null)
                {
                    throw;
                }

                // Create an instance of HttpResponseData with 500 status code.
                var newHttpResponse = httpReqData.CreateResponse(HttpStatusCode.Unauthorized);
                await newHttpResponse.WriteAsJsonAsync(new { Status = "Failed to authorize!" }, newHttpResponse.StatusCode);

                var invocationResult = context.GetInvocationResult();
                invocationResult.Value = newHttpResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
