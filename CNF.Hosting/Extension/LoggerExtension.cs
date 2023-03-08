using CNF.Common.Logger;
using Magicodes.ExporterAndImporter.Core.Extension;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Net.Http.Headers;
using LogLevel = NLog.LogLevel;

namespace CNF.Hosting.Extension;

public static class LoggerExtension
{
    public static void AddLoggerSetup(this WebApplicationBuilder webApplicationBuilder)
    {
        NLog.LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();
        NLog.LogManager.Configuration.Variables["connectionString"] = webApplicationBuilder.Configuration["ConnectionStrings:MySql"];
        //new DebugLog().HttpLog("123", "123333", "消息", LogLevel.Error);

        // webApplicationBuilder.Services.AddHttpLogging(logging =>
        // {
        //     logging.LoggingFields = HttpLoggingFields.RequestHeaders;
        //     // logging.RequestHeaders.Add("sec-ch-ua");
        //     // logging.ResponseHeaders.Add("MyResponseHeader");
        //     // logging.MediaTypeOptions.AddText("application/javascript");
        //     logging.RequestBodyLogLimit = 4096;
        //     logging.ResponseBodyLogLimit = 4096;
        // });
    }
}