using System.Diagnostics;
using CNF.Common.Logger;
using CNF.Domain.ValueObjects.Enums.Sys;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Extensions;
using Newtonsoft.Json;

namespace CNF.Infrastructure.Attributes;

/// <summary>
/// 操作日志记录
/// </summary>
public class LoggerAttribute : ActionFilterAttribute
{
    public string LogType { get; set; }
    private string ActionArguments { get; set; }
    
    
    private Stopwatch _stopwatch;

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        ActionArguments = JsonConvert.SerializeObject(context.ActionArguments);
        _stopwatch = new Stopwatch();
        _stopwatch.Start();
        base.OnActionExecuting(context);
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        base.OnActionExecuted(context);
        _stopwatch.Stop();
        var url = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
        var method = context.HttpContext.Request.Method;
        var qs = ActionArguments;
        var userName = context.HttpContext.User.Identity.Name;

        var msg = $"地址：{url} \n " +
                  $"方式：{method} \n " +
                  $"参数：{qs}\n " +
                  //$"结果：{res}\n " +
                  $"耗时：{_stopwatch.Elapsed.TotalMilliseconds} 毫秒";
        if (string.IsNullOrEmpty(LogType))
        {
            Dictionary<string, string> dic = new Dictionary<string, string>
            {
                { "POST", LoggerEnum.Add.GetDisplayName() },
                { "PUT", LoggerEnum.Update.GetDisplayName() },
                { "DELETE", LoggerEnum.Delete.GetDisplayName() },
                { "GET", LoggerEnum.Read.GetDisplayName() },
            };
            foreach (var item in dic)
            {
                if (method.Equals(item.Key, StringComparison.CurrentCultureIgnoreCase))
                {
                    LogType = item.Value;
                }
                else
                {
                    LogType = method;
                }
            }
        }

        DebugLog.DatabaseLog(userName, LogType, msg, NLog.LogLevel.Trace);
    }
}