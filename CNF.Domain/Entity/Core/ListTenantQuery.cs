namespace CNF.Domain.Entity.Core;

/// <summary>
/// 多租户列表查询
/// </summary>
public class ListTenantQuery: PageQuery, IGlobalTenant
{
    public int TenantId { get; set; }
}