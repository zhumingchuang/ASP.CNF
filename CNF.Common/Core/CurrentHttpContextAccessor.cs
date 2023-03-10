namespace CNF.Common.Core;

/// <summary>
/// 服务单例
/// </summary>
public static class SingletonServiceProvider
{
    public static IServiceProvider ServiceProvider;

    public static HttpContext? HttpContextAccessor
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