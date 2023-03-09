using CNF.Common.Core;
using CNF.Domain.Entity;

namespace CNF.API.Controllers;

/// <summary>
/// 用户扩展
/// </summary>
public static class UserExtension
{
    /// <summary>
    /// 更新登录信息
    /// </summary>
    public static void ModifyLoginInfo(this User user)
    {
        user.ModifyIpAddress();
        user.ModifyIsLogin();
        user.ModifyLastLoginTime();
    }
    
    /// <summary>
    /// 修改密码
    /// </summary>
    public static void ChangePassword(this User user,string password)
    {
        if (!string.IsNullOrEmpty(password))
        {
            user.Password = Md5Crypt.Encrypt(password);
        }
    }
    
    /// <summary>
    /// 修改登录地址
    /// </summary>
    public static void ModifyIpAddress(this User user)
    {
        user.Ip = CustomHttpContext.Current.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? CustomHttpContext.Current.Connection.RemoteIpAddress.ToString();
        user.Address = IpParseHelper.GetAddressByIP(user.Ip);
    }
    
    /// <summary>
    /// 更新登录时间
    /// </summary>
    public static void ModifyLastLoginTime(this User user)
    {
        user.LastLoginTime = DateTime.Now;
    }
    
    /// <summary>
    /// 更新登录状态
    /// </summary>
    public static void ModifyIsLogin(this User user)
    {
        user.IsLogin = true;
    }
}