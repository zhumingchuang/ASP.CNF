using System.ComponentModel;

namespace CNF.Domain.ValueObjects.Enums.Sys;

/// <summary>
/// 日志类型
/// </summary>
public enum LoggerEnum
{
    /// <summary>
    /// 登录
    /// </summary>
    [Description("登录")] Login = 1,

    /// <summary>
    /// 退出登录
    /// </summary>
    [Description("退出登录")] Logout = 2,

    /// <summary>
    /// 保存或添加
    /// </summary>
    [Description("保存或添加")] Add = 3,

    /// <summary>
    /// 更新
    /// </summary>
    [Description("更新/修改")] Update = 4,

    /// <summary>
    /// 更新
    /// </summary>
    [Description("审核")] Audit = 5,

    /// <summary>
    /// 删除
    /// </summary>
    [Description("删除")] Delete = 6,

    /// <summary>
    /// 读取/查询
    /// </summary>
    [Description("读取/查询")] Read = 7,

    /// <summary>
    /// 查看
    /// </summary>
    [Description("查看")] Look = 8,

    /// <summary>
    /// 更改状态
    /// </summary>
    [Description("更改状态")] Status = 9,

    /// <summary>
    /// 授权
    /// </summary>
    [Description("授权")] Authorize = 10,
}