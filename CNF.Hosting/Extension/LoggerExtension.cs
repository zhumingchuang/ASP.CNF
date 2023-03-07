using CNF.Common.Logger;
using LogLevel = NLog.LogLevel;

namespace CNF.Hosting.Extension;

public static class LoggerExtension
{
    public static void AddLoggerSetup(this IServiceCollection services, IConfiguration configuration)
    {
       var aaa= NLog.LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();
        NLog.LogManager.Configuration.Variables["connectionString"] = configuration["ConnectionStrings:MySql"];
        
        
        aaa.Log(LogLevel.Error,"asfsaqweqwewq2f");
        
        Console.WriteLine(configuration["ConnectionStrings:MySql"]+"数据库连接....");
        //DebugLog.Default.Log("测试一下日志消息",LogLevel.Error);
    }
}