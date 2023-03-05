﻿using CNF.API.Jwt.Extension;
using CNF.Common.Helper;
using CNF.Repository;
using CNF.Repository.Interface;
using SqlSugar;

namespace CNF.API;

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

        //jwt
        var jwtEnable = configuration["JwtConfig:IsEnable"];
        if (jwtEnable != null)
        {
            var boolean = Convert.ToBoolean(jwtEnable);
            if (boolean)
            {
                services.AddAuthorizationSetup(configuration);
            }
        }
        
        CustomHttpContext.ServiceProvider = services.BuildServiceProvider();
        
        services.AddDistributedMemoryCache();
    }
}