namespace CNF.Domain.Entity.Core;

/// <summary>
/// 租户列表查询基类
/// </summary>
public class PageQuery
{
    public int Page { get; set; } = 1;
    public int Limit { get; set; } = 15;
}