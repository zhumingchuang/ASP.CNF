using CNF.Common;
using CNF.Domain.Entity.Core;
using CNF.Repository.Interface;

namespace CNF.Domain.Repository;

public interface IRecycleRepository:IBaseRepository<IRecycle>
{
    Task<ApiResult> RestoreAsync(IDeletesInput deletesInput);
    Task<ApiResult> RealyDeleteAsync(IDeletesInput deletesInput);
    Task<ApiResult> SoftDeleteAsync<TEntity>(IDeletesTenantInput input, IBaseRepository<TEntity> service) where TEntity : BaseTenantEntity, new();
    Task<ApiResult> SoftDeleteAsync<TEntity>(IDeletesInput input, IBaseRepository<TEntity> service) where TEntity : BaseEntity, new();
    Task<ApiResult> GetPagesAsync(KeyListTenantQuery query);
}