namespace CNF.Domain.Entity.Core;

/// <summary>
/// 多租户
/// </summary>
public class BaseTenantEntity : BaseEntity, IGlobalTenant
{
    public int TenantId { get; set; }
    public string TenantName { get; set; }
}