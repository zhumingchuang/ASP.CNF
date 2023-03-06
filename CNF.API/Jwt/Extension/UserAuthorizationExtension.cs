using System.Security.Claims;
using CNF.API.Controllers.Core;

namespace CNF.API.Jwt.Extension;

/// <summary>
/// 用户权限扩展
/// </summary>
public static class UserAuthorizationExtension
{

    private static Func<IUser,List<Claim>> _func;
    private static IServiceCollection _service;
    public static void AddClaim<TUser>(this IServiceCollection service, Func<IUser,List<Claim>> func) where TUser : IUser
    {
        _func = func;
    }

    public static void GetClaim<TUser>(this IUser user) where TUser : IUser
    {
        _func.Invoke(user);
        
        _service.AddClaim<User>((useraa) =>
        {
            // ((User)useraa)
            return new List<Claim>();
        });

    }
}

public class User : IUser
{
    public string asasdasd;
}