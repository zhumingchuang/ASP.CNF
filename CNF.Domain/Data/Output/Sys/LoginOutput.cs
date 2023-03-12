namespace CNF.Domain.Data.Output.Sys;

/// <summary>
/// 登录输出
/// </summary>
public class LoginOutput
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Token
    /// </summary>
    public string Token { get; set; }
    
    /// <summary>
    /// 登陆时间
    /// </summary>
    public DateTime LastLoginTime { get; set; }
}