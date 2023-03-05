namespace CNF.Domain.Entity.Core;

public class DeletesTenantInput : DeletesInput, IGlobalTenant
{
    public int TenantId { get; set; }
}