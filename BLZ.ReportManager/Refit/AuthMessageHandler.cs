using System.Net.Http.Headers;
using BLZ.Client.Services;
using BLZ.ReportManager.Services;

namespace BLZ.ReportManager.Refit
{
    public class AuthMessageHandler : DelegatingHandler
    {
        private readonly AuthService _auth;

        public AuthMessageHandler(AuthService auth)
        {
            _auth = auth;
        }

        protected override async Task <HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancelToken)
        {
            HttpRequestHeaders headers = request.Headers;

            AuthenticationHeaderValue? authHeader = headers.Authorization;

            if(authHeader != null)
            {
                headers.Authorization = new AuthenticationHeaderValue(authHeader.Scheme, _auth.GetToken());
            }

            return await base.SendAsync(request, cancelToken);
        }
    }
}
