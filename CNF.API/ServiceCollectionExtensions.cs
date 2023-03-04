namespace CNF.API;

public static class ServiceCollectionExtensions
{
    public static void ApiSetup(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSqlsugarSetup
    }
}