using System.Text;
using CNF.Caches;
using CNF.Common.Core;
using CNF.Domain.ValueObjects.Enums.Sys;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace CNF.Domain.ValueObjects;

public class SysSetting
{
    /// <summary>
    /// 
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string EnName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Logo { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Copyright { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string LoginImg { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Favicon { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Remark { get; set; }

    public static Task WriteAsync(SysSetting input)
    {
        if (input == null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        var _cacheHelper = SingletonServiceProvider.HttpContextAccessor.RequestServices
            .GetRequiredService<IDistributedCache>();
        var setting = JsonConvert.SerializeObject(input);
        _cacheHelper.Set(nameof(ConfigEnum.Setting), input);
        return System.IO.File.WriteAllTextAsync(@"wwwroot/system.ini", setting, Encoding.UTF8);
    }

    public static async Task<SysSetting> ReadAsync()
    {
        var _cacheHelper = SingletonServiceProvider.HttpContextAccessor.RequestServices
            .GetRequiredService<IDistributedCache>();
        var value = _cacheHelper.Get<SysSetting>(nameof(ConfigEnum.Setting));
        if (value == null)
        {
            var valueStr = await System.IO.File.ReadAllTextAsync(@"wwwroot/system.ini", Encoding.UTF8);
            if (!string.IsNullOrEmpty(valueStr))
            {
                value = JsonConvert.DeserializeObject<SysSetting>(valueStr);
                _cacheHelper.Set(nameof(ConfigEnum.Setting), value);
            }
            else
            {
                value = new SysSetting();
            }
        }

        return value;
    }
}