using CNF.API.Jwt.Extension;
using CNF.Common.Core;
using CNF.Infrastructure;
using CNF.Repository;
using CNF.Repository.Interface;
using SqlSugar;

namespace CNF.API.Extension;

public static class ServiceCollectionExtensions
{
    public static void ApiSetup(this IServiceCollection services, IConfiguration configuration)
    {
        //sql
        services.AddSqlSugarSetup(option =>
        {
            option.ConnectionString = configuration["ConnectionStrings:MySql"];
            option.DbType = DbType.MySql;
            option.IsAutoCloseConnection = true;
            option.InitKeyType = InitKeyType.Attribute; //从特性读取主键自增信息
        });
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddAutoMapper(typeof(AutomapperProfile));
        //jwt
        services.AddAuthorizationSetup(configuration);
        
        services.AddScoped<ICurrentUserContext, CurrentUserContext>();

        SingletonServiceProvider.ServiceProvider = services.BuildServiceProvider();
        
        services.AddDistributedMemoryCache();
    }
}