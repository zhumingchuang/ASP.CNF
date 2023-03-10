using CNF.Common.Core;
using CNF.Domain.Entity;

namespace CNF.API.Controllers;

/// <summary>
/// 用户扩展
/// </summary>
public static class UserExtension
{
    /// <summary>
    /// 创建用户
    /// </summary>
    public static void CreateUser(this User user)
    {
        user.EncryptPassword();
        user.ModifyIpAddress();
    }

    /// <summary>
    /// 密码加密
    /// </summary>
    public static void EncryptPassword(this User user)
    {
        if (string.IsNullOrEmpty(user.Password))
        {
            throw new ArgumentException(nameof(user.Password));
        }

        user.Password = Md5Crypt.Encrypt(user.Password);
        if (user.Id > 0)
        {
            // NotifyModified();
        }
    }

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
    public static void ChangePassword(this User user, string password)
    {
        if (!string.IsNullOrEmpty(password))
        {
            user.Password = Md5Crypt.Encrypt(password);
        }
    }

    /// <summary>
    /// 修改密码
    /// </summary>
    public static void IsEquaPassword(this User user, string oldPassword, string confirmPassword)
    {
        if (string.IsNullOrEmpty(oldPassword))
        {
            throw new ArgumentException(nameof(oldPassword));
        }

        if (string.IsNullOrEmpty(confirmPassword))
        {
            throw new ArgumentException(nameof(confirmPassword));
        }

        oldPassword = Md5Crypt.Encrypt(oldPassword);
        if (user.Password != oldPassword)
        {
            throw new ArgumentException("旧密码错误!");
        }

        user.Password = Md5Crypt.Encrypt(user.Password);
    }

    /// <summary>
    /// 修改登录地址
    /// </summary>
    public static void ModifyIpAddress(this User user)
    {
        user.Ip = SingletonServiceProvider.HttpContextAccessor.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                  SingletonServiceProvider.HttpContextAccessor.Connection.RemoteIpAddress.ToString();
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