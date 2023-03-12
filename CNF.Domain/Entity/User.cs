using SqlSugar;

namespace CNF.Domain.Entity;

/// <summary>
/// 用户表
/// </summary>
[SugarTable("CNF_User")]
public class User
{
    public int Id { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 用户密码
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// 登录地址
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// IP地址
    /// </summary>
    public string Ip { get; set; }
    
    /// <summary>
    /// 登录时间
    /// </summary>
    public DateTime? LastLoginTime { get; set; }

    /// <summary>
    /// 是否删除
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// 是否登录
    /// </summary>
    public bool IsLogin { get; set; }
}