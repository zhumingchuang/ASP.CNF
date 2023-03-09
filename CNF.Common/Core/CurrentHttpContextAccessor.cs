namespace CNF.Common.Core;

public static class CurrentHttpContextAccessor
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