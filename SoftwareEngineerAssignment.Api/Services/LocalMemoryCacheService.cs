using Microsoft.Extensions.Caching.Memory;
using SoftwareEngineerAssignment.Api.Models.Config;

namespace SoftwareEngineerAssignment.Api.Services;

public class LocalMemoryCacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;

    public LocalMemoryCacheService(IMemoryCache cache, IConfiguration configuration)
    {
        _memoryCache = cache;
        CacheConfig = configuration.GetSection(CacheConfig.Key).Get<CacheConfig>();
    }

    public CacheConfig CacheConfig { get; }

    public T? Get<T>(object key)
    {
        _memoryCache.TryGetValue(key, out T? value);
        return value;
    }

    public void Set(object key, object value, MemoryCacheEntryOptions? options = null)
    {
        _memoryCache.Set(key, value, options);
    }
}