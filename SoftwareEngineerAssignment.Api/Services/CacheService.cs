using Microsoft.Extensions.Caching.Memory;

namespace SoftwareEngineerAssignment.Api.Services;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;

    public CacheService(IMemoryCache cache)
    {
        _memoryCache = cache;
    }

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