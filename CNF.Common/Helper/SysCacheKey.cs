namespace CNF.Common.Helper;

public class SysCacheKey
{
    /// <summary>
    /// 
    /// </summary>
    public const string CurrentTenant = "currentTenant";

    /// <summary>
    /// 用户登录非对称加密
    /// </summary>
    public const string LoginRsaCrypt = "loginRSACrypt";

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
    public const string EncryLoginKey = "encryLoginKey";

    /// <summary>
    /// 
    /// </summary>
    public const string OnlyLoginKey = "onlyLoginKey";

    /// <summary>
    ///  密码密钥
    /// </summary>
    public const string EncryptKey = "fenfenlg_salt_SmTRx";
}