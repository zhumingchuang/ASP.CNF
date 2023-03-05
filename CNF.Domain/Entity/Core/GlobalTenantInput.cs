namespace CNF.Domain.Entity.Core;

/// <summary>
/// 多租户  add  modify
/// </summary>
public class GlobalTenantInput:IGlobalTenant
{
    public int TenantId { get; set; }
}