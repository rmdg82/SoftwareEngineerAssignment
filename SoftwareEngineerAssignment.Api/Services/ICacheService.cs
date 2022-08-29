using Microsoft.Extensions.Caching.Memory;
using SoftwareEngineerAssignment.Api.Models.Config;

namespace SoftwareEngineerAssignment.Api.Services;

public interface ICacheService
{
    public CacheConfig CacheConfig { get; }

    T? Get<T>(object key);

    void Set(object key, object value, MemoryCacheEntryOptions? options = null);
}