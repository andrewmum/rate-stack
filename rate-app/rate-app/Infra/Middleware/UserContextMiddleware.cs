using FirebaseAdmin.Auth;
using Microsoft.Extensions.Caching.Memory;
using rate_it_api.Core.Entities;

namespace rate_app.Infra.Middleware
{
    public class UserContextMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _memoryCache;
        public UserContextMiddleware(RequestDelegate next, IMemoryCache cache)
        {
            _next = next;
            _memoryCache = cache;
        }

        public interface ICurrentUserService
        {
            string GetUserId();
            string GetEmail();
            string GetName();
            bool IsAuthenticated();
        }
        


    }
}
