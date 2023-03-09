using CNF.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace CNF.Infrastructure.Attributes;

public class ModelValidateAttribute:ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            //公共返回数据类
            ApiResult json = new ApiResult() { StatusCode = 200 };
            //获取具体的错误消息
            foreach (var item in context.ModelState.Values)
            {
                //遍历所有项目的中的所有错误信息
                foreach (var err in item.Errors)
                {
                    //消息拼接,用|隔开，前端根据容易解析
                    json.Msg += $"{err.ErrorMessage}|";
                }
            }
            json.StatusCode = StatusCodes.Status500InternalServerError;
            var setting = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
            context.HttpContext.Response.ContentType = "application/json;charset=utf-8";
            context.Result = new ObjectResult(JsonConvert.SerializeObject(json, Formatting.None, setting));
        }
    }
}