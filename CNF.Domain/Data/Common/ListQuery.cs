using CNF.Domain.Entity.Core;

namespace CNF.Domain.Dtos.Common;

public class ListQuery
{
    /// <summary>
    /// 列表查询基类（不是多租户的模块可以使用）
    /// </summary>
    public class PageQuery
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 15;
    }

    /// <summary>
    /// 多租户列表查询使用
    /// </summary>
    public class ListTenantQuery : PageQuery, IGlobalTenant
    {
        public int TenantId { get; set; }
    }

    /// <summary>
    /// 通用查询 ， 适合只有一个关键词查询的列表
    /// </summary>
    public class KeyListTenantQuery : ListTenantQuery, IGlobalTenant
    {
        public string Key { get; set; }
    }

    /// <summary>
    /// 适用于不是多租户的
    /// </summary>
    public class KeyListQuery : PageQuery
    {
        public string Key { get; set; }
    }
}