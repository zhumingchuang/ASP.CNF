using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace CNF.API.Jwt.Extension;

public static partial class AuthorizationExtension
{
    private static JwtSetting? _jwtSetting;
    
    public static void AddAuthorizationSetup(this IServiceCollection services, IConfiguration configuration)
    {
        //读取配置文件       
        _jwtSetting = configuration.GetSection("JwtSetting").Get<JwtSetting>();
        if (_jwtSetting == null)
        {
            throw new ArgumentNullException(nameof(JwtSetting));
        }
        
        // 令牌验证参数
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.SecurityKey)),
            ValidateIssuer = true,
            ValidIssuer = _jwtSetting.Issuer, //发行人
            ValidateAudience = true,
            ValidAudience = _jwtSetting.Audience, //订阅人
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(_jwtSetting.ExpireSeconds), //ClockSkew默认值为20s，它是一个缓冲期
            RequireExpirationTime = true,
        };

        // 开启Bearer认证
        services.AddAuthentication(o =>
            {
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = nameof(ApiResponseHandler);
                o.DefaultForbidScheme = nameof(ApiResponseHandler);
            })
            // 添加JwtBearer服务
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = tokenValidationParameters;
                o.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.Response.Headers.Add("Token-Error", context.ErrorDescription);
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

                        if (jwtToken.Issuer != _jwtSetting.Issuer)
                        {
                            context.Response.Headers.Add("Token-Error-Iss", "issuer is wrong!");
                        }

                        if (jwtToken.Audiences.FirstOrDefault() != _jwtSetting.Audience)
                        {
                            context.Response.Headers.Add("Token-Error-Aud", "Audience is wrong!");
                        }

                        // 如果过期，则把<是否过期>添加到，返回头信息中
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }

                        return Task.CompletedTask;
                    }
                };
            })
            .AddScheme<AuthenticationSchemeOptions, ApiResponseHandler>(nameof(ApiResponseHandler), o => { });
    }
}