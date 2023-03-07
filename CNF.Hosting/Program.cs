using CNF.Common.Extension;
using CNF.Common.Logger;
using CNF.Hosting.Extension;
using NLog.Extensions.Logging;
using NLog.Web;
using LogLevel = NLog.LogLevel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor ();
builder.Services.AddSignalR();

//日志
builder.Services.AddLoggerSetup(builder.Configuration);
builder.Logging.AddNLogWeb();
//api设置
builder.Services.ApiSetup(builder.Configuration);

//api文档
builder.Services.AddSwaggerSetup();

//路由配置
builder.Services.AddRouting(options => {
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = true;
    options.LowercaseQueryStrings = true;
});

//跨域配置
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    Console.WriteLine("等待eqweqwe");
    await Task.Delay(2000);

    Console.WriteLine("等待");
    new DebugLog().Log("测试一下日志消息123123",LogLevel.Trace);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();