using BLZ.ReportManager.Refit;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using Polly.Timeout;
using Refit;
using System.Text.Json;

namespace BLZ.Client.Refit
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddResilientApi<T>(this IServiceCollection service, string uri, int retryCount, TimeSpan retryWait, TimeSpan timeout) where T : class
        {
            service.AddTransient<AuthMessageHandler>();

            AsyncRetryPolicy<HttpResponseMessage> retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(retryCount, _ => retryWait);

            AsyncTimeoutPolicy<HttpResponseMessage> timeoutPolicy = Policy
                .TimeoutAsync<HttpResponseMessage>(timeout);

            RefitSettings settings = new()
            {
                Buffered = true,
                ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions(JsonSerializerDefaults.Web))
            };

            service
                .AddRefitClient<T>(settings)
                .AddPolicyHandler(retryPolicy)
                .AddPolicyHandler(timeoutPolicy)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(uri))
                .AddHttpMessageHandler<AuthMessageHandler>();

            return service;
        }
    }
}
