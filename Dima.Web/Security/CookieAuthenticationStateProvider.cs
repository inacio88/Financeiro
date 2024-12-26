using System.Net.Http.Json;
using System.Security.Claims;
using Dima.Core.Models.Account;
using Microsoft.AspNetCore.Components.Authorization;

namespace Dima.Web.Security
{
    public class CookieAuthenticationStateProvider(IHttpClientFactory clientFactory) : AuthenticationStateProvider, ICookieAuthenticationStateProvider
    {
        private bool _isAutheticated = false;
        private readonly HttpClient _client = clientFactory.CreateClient(Configuration.HttpClientName);
        public async Task<bool> CheckAuthenticatedAsync()
        {
            await GetAuthenticationStateAsync();
            return _isAutheticated;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            _isAutheticated = false;
            var user = new ClaimsPrincipal(new ClaimsIdentity());
            var userInfo = await GetUser();
            if(userInfo is null)
                return new AuthenticationState(user);

            var claims = await GetClaims(userInfo);
            var id = new ClaimsIdentity(claims, nameof(CookieAuthenticationStateProvider));
            user = new ClaimsPrincipal(id);
            
            _isAutheticated = true;

            return new AuthenticationState(user);
        }

        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        private async Task<User?> GetUser()
        {
            try
            {
                return await _client.GetFromJsonAsync<User?>("vi/identity/manage/info");
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        private async Task<List<Claim>> GetClaims(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
            };

            claims.AddRange(
                user.Claims.Where(x => x.Key != ClaimTypes.Name && x.Key != ClaimTypes.Email)
                            .Select(x => new Claim(x.Key, x.Value))
            );

            RoleClaim[]? roles;

            try
            {
                roles = await _client.GetFromJsonAsync<RoleClaim[]?>("v1/identity/roles");
            }
            catch
            {
                return claims;
            }

            foreach (var item in roles ?? [])
            {
                if (!string.IsNullOrEmpty(item.Type) && !string.IsNullOrEmpty(item.Value))
                    claims.Add(new Claim(item.Type, item.Value, item.ValueType, item.Issuer, item.OriginalIssuer));
            }

            return claims;
        }

    }
}