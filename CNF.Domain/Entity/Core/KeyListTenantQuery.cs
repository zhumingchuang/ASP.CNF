namespace CNF.Domain.Entity.Core;

/// <summary>
/// 多租户通用查询
/// </summary>
public class KeyListTenantQuery: ListTenantQuery, IGlobalTenant
{
    public string Key { get; set; }
}