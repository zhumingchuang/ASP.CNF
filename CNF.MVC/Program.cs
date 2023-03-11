using CNF.API.Extension;
using CNF.Common.Extension;
using CNF.Infrastructure.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddSignalR();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
    {
        o.Cookie.Name = "ShenNius.Mvc.Admin";
        o.LoginPath = new PathString("/sys/user/login");
        o.LogoutPath = new PathString("/sys/user/Logout");
        o.Cookie.HttpOnly = true;
    });

//日志
builder.AddLoggerSetup();

//api设置
builder.Services.ApiSetup(builder.Configuration);

//api文档
builder.Services.AddSwaggerSetup();

//路由配置
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = true;
    options.LowercaseQueryStrings = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStatusCodePagesWithReExecute("/error.html");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// 路由映射
app.UseEndpoints(endpoints =>
{
    //这个扩展方法全局添加也可以代替Authorize,如果因重写了IAuthorizationFilter就可以不添加。
    endpoints?.MapControllers().RequireAuthorization();
    endpoints.MapControllerRoute(
        name: "MyArea",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
    //全局路由配置
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=home}/{action=index}/{id?}");
    endpoints.MapHub<UserLoginNotifiHub>("userLoginNotifiHub");
});

app.Run();