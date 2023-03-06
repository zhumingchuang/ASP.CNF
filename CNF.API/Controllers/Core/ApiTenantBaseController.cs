// using AutoMapper;
// using CNF.Common;
// using CNF.Common.Helper;
// using CNF.Domain.Entity.Core;
// using CNF.Domain.Repository;
// using CNF.Domain.ValueObjects.Enums;
// using CNF.Infrastructure.Attributes;
// using CNF.Repository;
// using CNF.Repository.Interface;
// using Magicodes.ExporterAndImporter.Core;
// using Magicodes.ExporterAndImporter.Excel;
// using Microsoft.AspNetCore.Mvc;
// using SqlSugar;
//
// namespace CNF.API.Controllers;
//
// /// <summary>
// /// 多租用户
// /// </summary>
// /// <typeparam name="TEntity"></typeparam>
// /// <typeparam name="TDetailQuery"></typeparam>
// /// <typeparam name="TDeleteInput"></typeparam>
// /// <typeparam name="TListQuery"></typeparam>
// /// <typeparam name="TCreateInput"></typeparam>
// /// <typeparam name="TUpdateInput"></typeparam>
// [ApiController]
// [MultiTenant]
// public class ApiTenantBaseController<TEntity, TDetailQuery, TDeleteInput, TListQuery, TCreateInput,
//         TUpdateInput> : ControllerBase
//     where TEntity : BaseTenantEntity, new()
//     where TDetailQuery : DetailTenantQuery
//     where TDeleteInput : IDeletesTenantInput
//     where TListQuery : ListTenantQuery
//     where TCreateInput : class
//     where TUpdateInput : IEntity
// {
//     private readonly IBaseRepository<TEntity> _repository;
//     private readonly IMapper _mapper;
//
//     public ApiTenantBaseController(IBaseRepository<TEntity> repository, IMapper mapper)
//     {
//         _repository = repository;
//         _mapper = mapper;
//     }
//
//     /// <summary>
//     /// 批量真实删除
//     /// </summary>
//     /// <param name="deleteInput"></param>
//     /// <returns></returns>
//     [HttpDelete, Authority]
//     public virtual async Task<ApiResult> Deletes([FromBody] TDeleteInput deleteInput)
//     {
//         var res = await _repository.DeleteAsync(deleteInput.Ids);
//         return res <= 0 ? new ApiResult("删除失败了！") : new ApiResult();
//     }
//
//     /// <summary>
//     /// 单个彻底删除
//     /// </summary>
//     /// <param name="deleteInput"></param>
//     /// <returns></returns>
//     [HttpDelete, Authority]
//     public virtual async Task<ApiResult> Delete([FromBody] TDeleteInput deleteInput)
//     {
//         var res = await _repository.DeleteAsync(deleteInput.Ids);
//         return res <= 0 ? new ApiResult("删除失败了！") : new ApiResult();
//     }
//
//     /// <summary>
//     /// 软删除并将内容放回到回收站
//     /// </summary>
//     /// <param name="deleteInput"></param>
//     /// <returns></returns>
//     [HttpDelete, Authority(Action = nameof(BtnEnum.Recycle))]
//     public virtual Task<ApiResult> SoftDelete([FromBody] TDeleteInput deleteInput)
//     {
//         var recycleService = HttpContext.RequestServices.GetRequiredService<IRecycleRepository>();
//         return recycleService.SoftDeleteAsync(deleteInput, _repository);
//     }
//
//     /// <summary>
//     /// 列表分页
//     /// </summary>
//     /// <param name="listQuery">参数实体</param>
//     /// <returns></returns>
//     [HttpGet, Authority]
//     public virtual async Task<ApiResult> GetListPages([FromQuery] TListQuery listQuery)
//     {
//         var res = await LoadEntityPageListAsync(listQuery);
//         return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
//     }
//
//     [NonAction]
//     protected async Task<Page<TEntity>> LoadEntityPageListAsync(TListQuery listQuery)
//     {
//         var res = await _repository.GetPagesAsync(listQuery.Page, listQuery.Limit,
//             d => d.IsDeleted == false && d.TenantId == listQuery.TenantId, d => d.Id, false);
//         var db = HttpContext.RequestServices.GetService<ISqlSugarClient>();
//
//         var tenantList = await db.Queryable<Tenant>().Where(d => d.IsDeleted == false).ToListAsync();
//         foreach (var item in res.Items)
//         {
//             item.TenantName = tenantList.Where(d => d.Id == item.TenantId).Select(d => d.Name).FirstOrDefault();
//         }
//
//         return res;
//     }
//
//     /// <summary>
//     /// 详情
//     /// </summary>
//     /// <param name="detailQuery">参数实体</param>
//     /// <returns></returns>
//     [HttpGet, Authority]
//     public virtual async Task<ApiResult> Detail([FromQuery] TDetailQuery detailQuery)
//     {
//         var res = await _repository.GetModelAsync(d =>
//             d.IsDeleted == false && d.Id == detailQuery.Id && d.TenantId == detailQuery.TenantId);
//         return new ApiResult(data: res);
//     }
//
//     /// <summary>
//     /// 添加
//     /// </summary>
//     /// <param name="createInput">添加实体</param>
//     /// <returns></returns>
//     [HttpPost, Authority]
//     public virtual async Task<ApiResult> Add([FromBody] TCreateInput createInput)
//     {
//         var entity = _mapper.Map<TEntity>(createInput);
//         var res = await _repository.AddAsync(entity);
//         return res <= 0 ? new ApiResult("添加失败了！") : new ApiResult();
//     }
//
//     /// <summary>
//     /// 修改-默认忽略更新CreateTime字段
//     /// </summary>
//     /// <param name="updateInput">修改实体</param>
//     /// <returns></returns>
//     [HttpPut, Authority]
//     public virtual async Task<ApiResult> Modify([FromBody] TUpdateInput updateInput)
//     {
//         var entity = _mapper.Map<TEntity>(updateInput);
//         var model = await _repository.GetModelAsync(d => d.IsDeleted == false && d.Id == updateInput.Id);
//         if (model == null || (model?.Id) <= 0)
//         {
//             return new ApiResult($"该{updateInput.Id}为查询到对应的数据！");
//         }
//
//         var res = await _repository.UpdateAsync(entity, d => new { d.CreateTime });
//         return res <= 0 ? new ApiResult("修改失败了！") : new ApiResult();
//     }
//
//     /// <summary>
//     /// 生成实体EXCEL导入模板
//     /// </summary>
//     /// <returns></returns>
//     [HttpGet, Authority]
//     public virtual async Task<IActionResult> GenerateImportTemplate()
//     {
//         var fileName = await NPOIHelper.GenerateImportTemplate<TEntity>();
//         return File(System.IO.File.ReadAllBytes(fileName), "application/vnd.ms-excel", $"{typeof(TEntity).Name}.xlsx");
//     }
//
//     /// <summary>
//     /// 实体表数据导出到Excel
//     /// </summary>
//     /// <returns></returns>
//     /// <exception cref="ArgumentNullException"></exception>
//     [HttpGet, Authority]
//     public virtual async Task<IActionResult> Export([FromQuery] TListQuery listQuery)
//     {
//         IExporter exporter = new ExcelExporter();
//         var datas = await LoadEntityPageListAsync(listQuery);
//         if (datas.Items.Count <= 0)
//         {
//             throw new ArgumentNullException("集合没有可用数据导出！");
//         }
//
//         var name = typeof(TEntity).Name;
//         var result = await exporter.Export($"{name}.xlsx", datas.Items);
//         return File(System.IO.File.ReadAllBytes(result.FileName), "application/vnd.ms-excel", $"{name}.xlsx");
//     }
//
//     [HttpPost, Authority]
//     public virtual async Task<ApiResult> Import()
//     {
//         var file = Request.Form.Files[0];
//         var extensionName = Path.GetExtension(file.FileName);
//         if (!extensionName.ToLower().Equals(".xlsx"))
//         {
//             return new ApiResult("文件格式不正确,只支持XLSX文件！");
//         }
//
//         using (var stream = file.OpenReadStream())
//         {
//             IImporter Importer = new ExcelImporter();
//             var import = await Importer.Import<TEntity>(stream);
//             if (import?.Exception != null)
//             {
//                 return new ApiResult($"数据导入失败了！{import?.Exception}");
//             }
//
//             if (import.Data.Count > 0)
//             {
//                 var lists = import.Data.ToList();
//                 if (lists.Count > 0)
//                 {
//                     await _repository.AddListAsync(lists);
//                 }
//             }
//
//             return ApiResult.Successed($"{import.Data.Count}条数据已成功导入！");
//         }
//     }
// }