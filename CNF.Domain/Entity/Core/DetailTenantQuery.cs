namespace CNF.Domain.Entity.Core;

/// <summary>
/// 多租户查询
/// </summary>
public class DetailTenantQuery:DetailQuery,IGlobalTenant
{
    public int TenantId { get; set; }
}