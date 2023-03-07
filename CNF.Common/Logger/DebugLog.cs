using CNF.Common.Core;
using NLog;

namespace CNF.Common.Logger;

public class DebugLog
{
    readonly NLog.Logger _logger;

    private DebugLog(NLog.Logger logger)
    {
        _logger = logger;
    }
    
    public DebugLog(string name = "database") : this(LogManager.GetLogger(name))
    {

    }

    public static DebugLog Default { get; private set; }

    static DebugLog()
    {
        Default = new DebugLog(LogManager.GetCurrentClassLogger());
    }

    public void Log(string msg,NLog.LogLevel logLevel)
    {
        LogEventInfo lei = new LogEventInfo();
        lei.Level = logLevel;
        lei.Message = msg;
        _logger.Log(lei);
    }

    public void HttpLog(string userName, string Logger, string msg, NLog.LogLevel logLevel, Exception exception = null)
    {
        try
        {
            var accessor = new HttpContextAccessor();
            string ip = accessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                        accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            LogEventInfo lei = new LogEventInfo();
            lei.Properties["UserName"] = userName;
            lei.Properties["Logger"] = Logger;
            lei.Level = logLevel;
            lei.Message = msg;
            lei.Exception = exception;
            //TODO
            lei.Properties["Address"] = IpParseHelper.GetAddressByIP(ip);
            _logger.Log(lei);
        }
        catch
        {
            // ignored
        }
    }
}