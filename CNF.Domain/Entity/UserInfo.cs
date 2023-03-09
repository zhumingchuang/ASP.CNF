using SqlSugar;

namespace CNF.Domain.Entity;

/// <summary>
/// 用户信息表
/// </summary>
[SugarTable("CNF_UserInfo")]
public class UserInfo
{
    /// <summary>
    /// ID
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string Nickname { get; private set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; private set; }

    /// <summary>
    /// 性别
    /// </summary>
    public string Sex { get; private set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string HeadImg { get; private set; }
}