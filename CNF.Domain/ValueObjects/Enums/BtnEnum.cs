using System.ComponentModel;

namespace CNF.Domain.ValueObjects.Enums;

public enum BtnEnum
{
    /// <summary>
    /// 详情
    /// </summary>
    [Description("详情")]
    Detail,
    /// <summary>
    /// 导出
    /// </summary>
    [Description("导出")]
    Export,
    /// <summary>
    /// 导入
    /// </summary>
    [Description("导入")]
    Import,
    /// <summary>
    /// 删除
    /// </summary>
    [Description("删除")]
    Delete,
    /// <summary>
    /// 编辑
    /// </summary>
    [Description("编辑")]
    Edit,
    /// <summary>
    /// 添加
    /// </summary>
    [Description("添加")]
    Add,
    /// <summary>
    /// 授权
    /// </summary>
    [Description("授权")]
    Auth,
    /// <summary>
    /// 批量删除
    /// </summary>
    [Description("批量删除")]
    Deletes,
    /// <summary>
    /// 放入回收站
    /// </summary>
    [Description("放入回收站")]
    Recycle
}