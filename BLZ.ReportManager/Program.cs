// See https://aka.ms/new-console-template for more information
using BLZ.Client.Refit;
using BLZ.Client.Services;
using BLZ.ReportManager.Refit;
using BLZ.ReportManager.Render;
using BLZ.ReportManager.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RestfulFirebase;
using RestfulFirebase.Auth;

IServiceCollection services = new ServiceCollection();

services
    .AddSingleton<FirebaseConfig>(new FirebaseConfig("AIzaSyA4Gv7YulJ3SSH8CkCje7foJsG55nJ3RVg"))
    .AddSingleton<RestfulFirebaseApp>()
    .AddSingleton<AuthApp>(i => i.GetRequiredService<RestfulFirebaseApp>().Auth);

services
    .AddSingleton<AuthService>()
    .AddSingleton<InputService>()
    .AddSingleton<RenderService>()
    .AddSingleton<RenderManager>();

services
    .AddSingleton<RenderAuth>()
    .AddSingleton<RenderReportOption>();

var apiRetryCount = 3;
var apiRetryWait = TimeSpan.FromSeconds(1);
var apiTimeout = TimeSpan.FromSeconds(10);
var apiUrl = "https://blazecart.azurewebsites.net/api";

services
    .AddResilientApi<IReportingApi>(apiUrl, apiRetryCount, apiRetryWait, apiTimeout);

services.AddLogging(b => {
    b.AddSimpleConsole(format =>
    {
        format.SingleLine = true;
        format.IncludeScopes = false;
    });
    b.SetMinimumLevel(LogLevel.Debug);
});

var provider = services
    .BuildServiceProvider();

await provider.GetRequiredService<RenderService>().Run();
