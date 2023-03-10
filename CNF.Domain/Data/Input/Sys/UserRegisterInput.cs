namespace CNF.Domain.Data.Input.Sys;

/// <summary>
/// 用户注册
/// </summary>
public class UserRegisterInput
{
    /// <summary>
    /// 用户
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 登录密码
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public string Sex { get; set; } = "男";

    /// <summary>
    /// 手机号码
    /// </summary>
    public string Mobile { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }
}