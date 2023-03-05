namespace CNF.Domain.Entity.Core;

/// <summary>
/// 多租户
/// </summary>
public interface IGlobalTenant
{
    public int TenantId { get; set; }
}