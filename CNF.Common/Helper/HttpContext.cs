using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CNF.Common.Helper;

public class CustomHttpContext
{
    public static IServiceProvider ServiceProvider;

    public static HttpContext Current
    {
        get
        {
            var httpContextAccessor = ServiceProvider.GetService<IHttpContextAccessor>();
            if (httpContextAccessor == null)
            {
                throw new ArgumentNullException(nameof(httpContextAccessor));
            }

            return httpContextAccessor?.HttpContext;
        }
    }
}