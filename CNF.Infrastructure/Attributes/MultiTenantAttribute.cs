using CNF.Domain.Entity.Core;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace CNF.Infrastructure.Attributes;

public class MultiTenantAttribute: ActionFilterAttribute, IActionFilter
{
    /// <summary>
    /// 全局注册过滤器 ，自动为添加 更新方法赋值。也可自行手动打上特性标签
    /// </summary>
    /// <param name="context"></param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
        // IDistributedCache cache = context.HttpContext.RequestServices.GetRequiredService<IDistributedCache>();
        ICurrentUserContext currentUserContext = context.HttpContext.RequestServices.GetRequiredService<ICurrentUserContext>();
        // var tenantId = cache.Get<int>($"{SysCacheKey.CurrentTenant}:{currentUserContext.Id}");
        foreach (var parameter in actionDescriptor.Parameters)
        {
            var parameterName = parameter.Name;
            var parameterType = parameter.ParameterType;

            //自动添加租户id
            if (typeof(IGlobalTenant).IsAssignableFrom(parameterType))
            {
                var model = context.ActionArguments[parameterName] as IGlobalTenant;
                if (model != null)
                {
                    if (currentUserContext.TenantId == 0)
                    {
                        //进程内缓存重启后多租户值会丢失！使用持久化的nosql可解决。如果用用进程内缓存，则退出重新登录赋值。
                        throw new ArgumentNullException("缓存获取不到当前的租户值!请退出重新登录！");
                    }
                    model.TenantId = currentUserContext.TenantId;
                }
            }
        }
    }
}