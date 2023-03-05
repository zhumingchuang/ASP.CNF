using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace CNF.Repository;

public static class ServiceCollectionExtensions
{
    public static void AddSqlSugarSetup(this IServiceCollection services, Action<ConnectionConfig> setupAction)
    {
        if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));
        
        ConnectionConfig connectionConfig = new ConnectionConfig ();
        setupAction (connectionConfig);
        if (connectionConfig == null)throw new ArgumentNullException (nameof(ConnectionConfig));
        
        SqlSugarScope implementationInstance = new SqlSugarScope (new ConnectionConfig
        {
            DbType = connectionConfig.DbType,
            ConnectionString = connectionConfig.ConnectionString,
            IsAutoCloseConnection = connectionConfig.IsAutoCloseConnection,
            InitKeyType = connectionConfig.InitKeyType
        }, delegate (SqlSugarClient db)
        {
            db.Aop.OnLogExecuting = delegate (string sql, SugarParameter[] pars)
            {
                Console.WriteLine (sql);
            };
            db.Aop.OnError = delegate (SqlSugarException exp)
            {
                string sql2 = exp.Sql;
            };
        });
        services.AddSingleton ((ISqlSugarClient)implementationInstance);
    }
}