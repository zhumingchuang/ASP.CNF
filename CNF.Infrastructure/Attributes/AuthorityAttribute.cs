using CNF.Common;
using CNF.Common.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using CNF.Domain.Dtos.Output.Sys;
using Newtonsoft.Json;
using CNF.Caches;

namespace CNF.Infrastructure.Attributes;

public class AuthorityAttribute : ActionFilterAttribute
{
    public string Action { get; set; }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.HttpContext.User.Identity.IsAuthenticated)
        {
            ReturnResult(context, "很抱歉,您未登录！", StatusCodes.Status401Unauthorized);
            return;
        }
        //当用户名为mhg时（超级管理员），不用验证权限。
#if DEBUG
        var currentName = context.HttpContext.User.Identity.Name;
        if (currentName.Equals("mhg2"))
        {
            return;
        }
#endif

        var controller = context.ActionDescriptor.RouteValues["controller"].ToString();
        var action = context.ActionDescriptor.RouteValues["action"].ToString();
        var method = context.HttpContext.Request.Method;
        if (string.IsNullOrEmpty(controller) || string.IsNullOrEmpty(action) || string.IsNullOrEmpty(method))
        {
            ReturnResult(context, "controller and action and method is not found", StatusCodes.Status403Forbidden);
            return;
        }

        IDistributedCache cache =
            context.HttpContext.RequestServices.GetRequiredService(typeof(IDistributedCache)) as IDistributedCache;
        var userId = context.HttpContext.User.Claims.FirstOrDefault(d => d.Type == JwtRegisteredClaimNames.Sid).Value;
        //从缓存获得权限

        var list = cache.Get<List<MenuAuthOutput>>($"{SysCacheKey.AuthMenu}:{userId}");
        if (list == null || list.Count <= 0)
        {
            ReturnResult(context, "不好意思，您暂无该资源操作权限，请联系系统管理员或退出系统重新登录！", StatusCodes.Status403Forbidden);
            return;
        }

        //1、验证列表(前端校验)、只验证列表权限码，数据库中存储的权限码对应的就是controller，数据库实体名称，后续开发保持一致，因为列表授权时，参数url为页面展示的地址。
        var model = list.FirstOrDefault(d => d.NameCode.Equals(controller, StringComparison.OrdinalIgnoreCase));
        if (model == null)
        {
            ReturnResult(context, "不好意思，您没有该列表操作权限", StatusCodes.Status403Forbidden);
            return;
        }

        //2、验证列表按钮
        //2.1 不验证列表权限的action 过滤掉
        if (action.Equals("GetListPages", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        if (!string.IsNullOrEmpty(model.BtnCodeName))
        {
            var arryBtn = model.BtnCodeName.Split(',');
            if (arryBtn.Length > 0)
            {
                if (!string.IsNullOrEmpty(Action))
                {
                    action = Action;
                }

                if (arryBtn.FirstOrDefault(d => d == action.ToLower()) == null)
                {
                    ReturnResult(context, "不好意思，您没有该按钮操作权限", StatusCodes.Status403Forbidden);
                    return;
                }
            }
        }

        base.OnActionExecuting(context);
    }

    private static void ReturnResult(ActionExecutingContext context, string msg, int statusCodes)
    {
        context.HttpContext.Response.ContentType = "application/json;charset=utf-8";
        var setting = new JsonSerializerSettings
        {
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };
        context.Result = new JsonResult(new ApiResult(msg, statusCodes), setting);
    }
}