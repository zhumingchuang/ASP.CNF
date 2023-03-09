using System.Text.RegularExpressions;

namespace CNF.Common.Core;

public enum EIdentityType
{
    None,

    /// <summary>
    /// 手机
    /// </summary>
    Phone,

    /// <summary>
    /// 邮箱
    /// </summary>
    Email,
}

/// <summary>
/// 身份类型
/// </summary>
public static class IdentityTypeHelp
{
    public static EIdentityType GetIdentityType(string input)
    {
        Regex regex = new Regex("^13\\d{9}$");
        if (regex.IsMatch(input))
        {
            return EIdentityType.Phone;
        }

        Regex regEmail = new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$");
        if (regEmail.IsMatch(input))
        {
            return EIdentityType.Email;
        }

        return EIdentityType.None;
    }
}