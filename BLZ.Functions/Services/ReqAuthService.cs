using Microsoft.Azure.Functions.Extensions.JwtCustomHandler;
using Microsoft.Azure.Functions.Extensions.JwtCustomHandler.Interface;
using Microsoft.Azure.Functions.Worker.Http;
using System.Security.Claims;

namespace BLZ.Functions.Services
{
    public class ReqAuthService
    {
        private readonly IFirebaseTokenProvider _tokenProvider;

        public ReqAuthService(IFirebaseTokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        public async Task<ClaimsPrincipal?> GetPrincipal(HttpRequestData req)
        {
            if (req.Headers is null || req.Body is null)
                return null;

            var tokenResult = await _tokenProvider.ValidateToken(req);
            if (tokenResult.Status != AccessTokenStatus.Valid)
                return null;

            return tokenResult.Principal;
        }

        public async Task<bool> IsAuthorized(HttpRequestData req)
        {
            return await GetPrincipal(req) != null;
        }
    }
}
