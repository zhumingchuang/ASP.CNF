using AutoMapper;
using CNF.Common;
using CNF.Domain.Entity.Core;
using CNF.Infrastructure.Attributes;
using CNF.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CNF.API.Controllers.Core;

/// <summary>
/// 非多租用户模块
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TDetailQuery"></typeparam>
/// <typeparam name="TDeleteInput"></typeparam>
/// <typeparam name="TListQuery"></typeparam>
/// <typeparam name="TCreateInput"></typeparam>
/// <typeparam name="TUpdateInput"></typeparam>
public class ApiBaseController<TEntity, TDetailQuery, TDeleteInput, TListQuery, TCreateInput, TUpdateInput> : ControllerBase
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

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="commonDeleteInput"></param>
    /// <returns></returns>
    [HttpDelete, Authority]
    public virtual async Task<ApiResult> Deletes([FromBody] TDeleteInput commonDeleteInput)
    {
        var res = await _repository.DeleteAsync(commonDeleteInput.Ids);
        return res <= 0 ? new ApiResult("删除失败了！") : new ApiResult();
    }

    [HttpGet, Authority]
    public virtual async Task<ApiResult> GetListPages([FromQuery] TListQuery listQuery)
    {
        var res = await _repository.GetPagesAsync(listQuery.Page, listQuery.Limit, d => d.IsDeleted == false, d => d.Id,
            false);
        return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
    }

    [HttpGet, Authority]
    public virtual async Task<ApiResult> Detail([FromQuery] TDetailQuery detailQuery)
    {
        var res = await _repository.GetModelAsync(d => d.Id == detailQuery.Id && d.IsDeleted == false);
        return new ApiResult(data: res);
    }

    [HttpPost, Authority]
    public virtual async Task<ApiResult> Add([FromBody] TCreateInput createInput)
    {
        var entity = _mapper.Map<TEntity>(createInput);
        var res = await _repository.AddAsync(entity);
        return res <= 0 ? new ApiResult("添加失败了！") : new ApiResult();
    }

    [HttpPut, Authority]
    public virtual async Task<ApiResult> Modify([FromBody] TUpdateInput updateInput)
    {
        var entity = _mapper.Map<TEntity>(updateInput);
        var model = await _repository.GetModelAsync(d => d.IsDeleted == false && d.Id == updateInput.Id);
        if (model == null || (model?.Id) <= 0)
        {
            return new ApiResult($"该{updateInput.Id}为查询到对应的数据！");
        }

        var res = await _repository.UpdateAsync(entity, d => new { d.CreateTime });
        return res <= 0 ? new ApiResult("修改失败了！") : new ApiResult();
    }
}