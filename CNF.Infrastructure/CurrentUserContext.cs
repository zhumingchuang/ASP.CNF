using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using CNF.Caches;
using CNF.Common.Core;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CNF.Infrastructure;

/// <summary>
/// 用户上下文
/// </summary>
public interface ICurrentUserContext
{
    /// <summary>
    /// 用户id
    /// </summary>
    int Id { get; }

    /// <summary>
    /// 多租户Id
    /// </summary>
    int TenantId { get; }

    /// <summary>
    /// 用户昵称
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 用户手机号
    /// </summary>
    string Mobile { get; }
    
    /// <summary>
    /// 是否登录
    /// </summary>
    /// <returns></returns>
    bool IsAuthenticated();
    
    
    
    IEnumerable<Claim> GetClaimsIdentity();
    List<string> GetClaimValueByType(string claimType);

    string GetToken();
    List<string> GetUserInfoFromToken(string claimType);

    bool IsAdmin();

    bool InRole(string roleType);
}

/// <summary>
/// 用户上下文
/// </summary>
public class CurrentUserContext : ICurrentUserContext
{
    private readonly IHttpContextAccessor _accessor;
    private readonly IDistributedCache _cacheHelper;

    public CurrentUserContext(IHttpContextAccessor accessor, IDistributedCache cacheHelper)
    {
        _accessor = accessor;
        _cacheHelper = cacheHelper;
    }

    public int TenantId => GetTenantId();

    private int GetTenantId()
    {
        var tenant = _cacheHelper.Get<int>($"{SysCacheKey.CurrentTenant}:{Id}");
        if (tenant <= 0)
        {
            throw new ArgumentNullException("当前租户值为空！");
        }

        return tenant;
    }

    public string Name => GetName();

    private string GetName()
    {
        if (IsAuthenticated())
        {
            return _accessor.HttpContext.User.Identity.Name;
        }
        else
        {
            if (!string.IsNullOrEmpty(GetToken()))
            {
                return GetUserInfoFromToken("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")
                    .FirstOrDefault().ToString();
            }
        }

        return "";
    }

    public string Mobile => GetClaimValueByType("mobile").FirstOrDefault()?.ToString();

    public int Id => Convert.ToInt32(GetClaimValueByType(JwtRegisteredClaimNames.Sid).FirstOrDefault());

    public bool IsAuthenticated()
    {
        if (_accessor.HttpContext?.User != null)
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        return false;
    }


    public string GetToken()
    {
        return _accessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
    }

    public List<string> GetUserInfoFromToken(string claimType)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        if (!string.IsNullOrEmpty(GetToken()))
        {
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(GetToken());

            return (from item in jwtToken.Claims
                where item.Type == claimType
                select item.Value).ToList();
        }
        else
        {
            return new List<string>() { };
        }
    }

    public IEnumerable<Claim> GetClaimsIdentity()
    {
        return _accessor.HttpContext.User.Claims;
    }

    public List<string> GetClaimValueByType(string claimType)
    {
        return (from item in GetClaimsIdentity()
            where item.Type == claimType
            select item.Value).ToList();
    }

    public bool IsAdmin()
    {
        return GetClaimsIdentity().FirstOrDefault(x => x.Type.Equals("IsAdmin"))?.Value ==
               "1";
    }

    public bool InRole(string roleType)
    {
        return GetClaimsIdentity().FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value ==
               roleType;
    }
}