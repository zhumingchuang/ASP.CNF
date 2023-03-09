using SqlSugar;

namespace CNF.Domain.Entity;

/// <summary>
/// 用户身份验证
/// </summary>
[SugarTable("CNF_UserAuths")]
public class UserAuths
{
    /// <summary>
    /// ID
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public int UserId { get; private set; }

    /// <summary>
    /// 身份类型
    /// </summary>
    public string Identity_Type { get; private set; }

    /// <summary>
    /// 标识
    /// </summary>
    public string Identifier { get; private set; }
    
    /// <summary>
    /// 凭据
    /// </summary>
    public string Credential { get; private set; }
}