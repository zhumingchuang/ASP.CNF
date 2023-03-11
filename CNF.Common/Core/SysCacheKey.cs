namespace CNF.Common.Core;

public class SysCacheKey
{
    /// <summary>
    /// 登录加密Key
    /// </summary>
    public const string LoginRsaCrypt = "loginRSACrypt";
    
    /// <summary>
    /// MVC登录加密Key
    /// </summary>
    public const string EncryLoginKey = "encryLoginKey";
    
    /// <summary>
    /// 
    /// </summary>
    public const string CurrentTenant = "currentTenant";

    /// <summary>
    /// 当前用户拥有的所有权限去做校验
    /// </summary>
    public const string AuthMenu = "authMenu";

    /// <summary>
    /// 左侧树形菜单 key=$“MenuTrees:{userId}”
    /// </summary>
    public const string MenuTrees = "MenuTrees";
    
    /// <summary>
    /// 
    /// </summary>
    public const string OnlyLoginKey = "onlyLoginKey";

    /// <summary>
    ///  密码密钥
    /// </summary>
    public const string EncryptKey = "fenfenlg_salt_SmTRx";
}