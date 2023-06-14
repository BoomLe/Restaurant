using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.Service.APICart.Utility
{
    public class BackendApiAuthenHttpClientHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _accessor;
        public BackendApiAuthenHttpClientHandler(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) 
        {
            var token = await _accessor.HttpContext.GetTokenAsync("access_token");

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
