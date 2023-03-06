using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CNF.API.Controllers.Core;
using Microsoft.IdentityModel.Tokens;

namespace CNF.API.Jwt.Extension;

/// <summary>
/// 用户权限扩展
/// </summary>
public static partial class AuthorizationExtension
{
    private static Delegate? _addClaimFunc;

    public static void AddClaim<TUser>(this IServiceCollection service, Func<TUser, List<Claim>> addClaimFunc)
        where TUser : IUser
    {
        _addClaimFunc = addClaimFunc;
    }

    public static List<Claim>? GetClaim<TUser>(this TUser user) where TUser : IUser
    {
        return ((Func<TUser, List<Claim>>)_addClaimFunc!)?.Invoke((TUser)user);
    }

    public static string GetJwtToken<TUser>(this TUser user) where TUser : IUser
    {
        var claim= GetClaim(user);
        if (claim == null) throw new Exception();
        var token = BuildJwtToken(claim);
        return token;
    }

    public static string BuildJwtToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting!.SecurityKey));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var jwtToken = new JwtSecurityToken(
            issuer: _jwtSetting.Issuer,
            audience: _jwtSetting.Audience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddMinutes(1),
            signingCredentials: signingCredentials
        );
        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}