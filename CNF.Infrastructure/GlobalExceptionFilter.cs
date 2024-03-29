﻿using CNF.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;

namespace CNF.Infrastructure;

/// <summary>
///  异常过滤器
/// </summary>
public class GlobalExceptionFilter: IExceptionFilter
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    private readonly ILogger<GlobalExceptionFilter> _logger;
    public GlobalExceptionFilter(IWebHostEnvironment webHostEnvironment, ILogger<GlobalExceptionFilter> logger)
    {
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;
    }
    
    public void OnException(ExceptionContext context)
    {
        var json = new ApiResult { StatusCode = 500, Success = false, Msg = context.Exception.Message };
        var errorAudit = "Unable to resolve service for";
        if (!string.IsNullOrEmpty(json.Msg) && json.Msg.Contains(errorAudit))
        {
            json.Msg = json.Msg.Replace(errorAudit, $"（若新添加服务，需要重新编译项目）{errorAudit}");
        }
        try
        {
            if (_webHostEnvironment.IsDevelopment())
            {
                json.Msg = context.Exception?.Message;//显示堆栈信息  
                string msg = json.Msg + "\r\n" + context.Exception.StackTrace;
                _logger.LogError(msg);
                //LogHelper.Default.Process("", "", json.Msg, NLog.LogLevel.Debug);
            }
            else
            {
                json.Msg = context.Exception.Message;
                //new LogHelper().Process("", "", json.Msg, NLog.LogLevel.Error, context.Exception);
            }
        }
        catch
        {
        }

        json.StatusCode = StatusCodes.Status500InternalServerError;
        var setting = new JsonSerializerSettings
        {
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };
        context.Result = new InternalServerErrorObjectResult(JsonConvert.SerializeObject(json, Formatting.None, setting));

    }
}

public class InternalServerErrorObjectResult : ObjectResult
{
    public InternalServerErrorObjectResult(object value) : base(value)
    {
        StatusCode = StatusCodes.Status500InternalServerError;
    }
}