using AutoMapper;
using CNF.Common;
using CNF.Domain.Entity.Core;
using CNF.Infrastructure.Attributes;
using CNF.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CNF.API.Controllers.Core;

/// <summary>
/// 非多租户控制器基类
/// </summary>
public class
    ApiBaseController<TEntity, TDetailQuery, TDeleteInput, TListQuery, TCreateInput, TUpdateInput> : ControllerBase
    where TEntity : BaseEntity, new()
    where TDeleteInput : DeletesInput
    where TDetailQuery : DetailQuery
    where TListQuery : PageQuery
    where TUpdateInput : IEntity
{
    private readonly IBaseRepository<TEntity> _repository;
    private readonly IMapper _mapper;
    public ApiBaseController(IBaseRepository<TEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    [HttpDelete, Authority]
    public virtual async Task<ApiResult> Deletes([FromBody] TDeleteInput commonDeleteInput)
    {
        var res = await _repository.DeleteAsync(commonDeleteInput.Ids);
        return res <= 0 ? new ApiResult("删除失败了！") : new ApiResult();
    }
}