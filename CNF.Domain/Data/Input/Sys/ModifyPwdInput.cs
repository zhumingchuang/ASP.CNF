namespace CNF.Domain.Data.Input.Sys;

/// <summary>
/// 修改密码
/// </summary>
public class ModifyPwdInput
{
    /// <summary>
    /// ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 新密码
    /// </summary>
    public string NewPassword { get; set; }

    /// <summary>
    /// 确认密码
    /// </summary>
    public string ConfirmPassword { get; set; }

    /// <summary>
    /// 旧密码
    /// </summary>
    public string OldPassword { get; set; }

    /// <summary>
    /// 密码修改
    /// </summary>
    public void ModifyPassword(int userId)
    {
        if (Id == 0)
        {
            Id = userId;
        }

        if (!ConfirmPassword.Equals(NewPassword))
        {
            throw new ArgumentException("两次输入的密码不一致");
        }
    }
}