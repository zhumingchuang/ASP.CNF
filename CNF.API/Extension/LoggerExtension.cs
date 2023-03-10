using NLog.Web;

namespace CNF.API.Extension;

public static class LoggerExtension
{
    public static void AddLoggerSetup(this WebApplicationBuilder webApplicationBuilder)
    {
        NLog.LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();
        NLog.LogManager.Configuration.Variables["connectionString"] = webApplicationBuilder.Configuration["ConnectionStrings:MySql"];
        webApplicationBuilder.Logging.AddNLogWeb();


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