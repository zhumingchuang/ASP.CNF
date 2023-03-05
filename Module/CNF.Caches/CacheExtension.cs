using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace CNF.Caches;

public static class CacheExtension
{
    public static T Get<T>(this IDistributedCache distributedCache, string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key));
        }
        var value = distributedCache.GetString(key);
        if (!string.IsNullOrEmpty(value))
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
        return default(T);
    }
    
    public static async Task<T> GetAsync<T>(this IDistributedCache distributedCache, string key, CancellationToken token = default(CancellationToken))
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key));
        }
        var value = await distributedCache.GetStringAsync(key, token);
        if (!string.IsNullOrEmpty(value))
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
        return default(T);
    }
    
    public static void Set<T>(this IDistributedCache distributedCache, string key, T value, DistributedCacheEntryOptions options = null)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key));
        }
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        var values = JsonConvert.SerializeObject(value);
        if (options != null)
        {
            distributedCache.SetString(key, values, options);
        }
        else
        {
            distributedCache.SetString(key, values);
        }
    }
    
    public static async Task SetAsync<T>(this IDistributedCache distributedCache, string key, T value, DistributedCacheEntryOptions options = null)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key));
        }
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        var values = JsonConvert.SerializeObject(value);
        if (options != null)
        {
            await distributedCache.SetStringAsync(key, values, options);
        }
        else
        {
            await distributedCache.SetStringAsync(key, values);
        }
    }
    
    public static void Set<T>(this IDistributedCache distributedCache, string key, T value, TimeSpan timeSpan)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key));
        }
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        var values = JsonConvert.SerializeObject(value);
        DistributedCacheEntryOptions entryOptions = new DistributedCacheEntryOptions();
        entryOptions.SlidingExpiration = timeSpan;
        distributedCache.SetString(key, values, entryOptions);
    }
}