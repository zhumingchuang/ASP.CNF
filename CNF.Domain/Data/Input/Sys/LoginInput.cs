namespace CNF.Domain.Data.Input.Sys;

/// <summary>
/// 登录输入
/// </summary>
public class LoginInput
{
    /// <summary>
    /// ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 登录密码
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Guid
    /// </summary>
    public string NumberGuid { get; set; }

    /// <summary>
    /// 确定再次登录
    /// </summary>
    public bool ConfirmLogin { get; set; }
}