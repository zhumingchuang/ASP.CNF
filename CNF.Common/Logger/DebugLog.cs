using CNF.Common.Core;
using NLog;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace CNF.Common.Logger;

public static class DebugLog
{
    public static NLog.Logger DefaultLogger { get; } = LogManager.GetCurrentClassLogger();
    public static NLog.Logger Database { get; } = LogManager.GetLogger("database");

    /// <summary>
    /// 日志
    /// </summary>
    public static void Log(string msg, NLog.LogLevel logLevel, Exception exception = null)
    {
        LogEventInfo lei = new LogEventInfo();
        lei.Level = logLevel;
        lei.Message = msg;
        lei.Exception = exception;
        DefaultLogger.Log(lei);
    }

    /// <summary>
    /// 数据库记录日志
    /// </summary>
    public static void DatabaseLog(string userName, string loggerType, string msg, NLog.LogLevel logLevel,
        Exception exception = null)
    {
        try
        {
            var _accessor = new HttpContextAccessor();
            string ip = _accessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                        _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            LogEventInfo lei = new LogEventInfo();
            lei.Properties["UserName"] = userName;
            lei.Properties["LoggerType"] = loggerType;
            lei.Level = logLevel;
            lei.Message = msg;
            lei.Exception = exception;
            lei.Properties["Address"] = IpParseHelper.GetAddressByIP(ip);
            Database.Log(lei);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}