using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CNF.API.Jwt;

public class JwtHelper
{
    private readonly IOptions<JwtSetting> _jwtSetting;

    public JwtHelper(IOptions<JwtSetting> jwtSetting)
    {
        _jwtSetting = jwtSetting;
    }

    /// <summary>
    /// 获取JwtSecurityToken
    /// </summary>
    public string BuildJwtToken(Claim[] claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Value.SecurityKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var jwtToken = new JwtSecurityToken(
            issuer: _jwtSetting.Value.Issuer,
            audience: _jwtSetting.Value.Audience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddHours(24),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}