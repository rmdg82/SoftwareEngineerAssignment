using Microsoft.Extensions.Caching.Memory;

namespace SoftwareEngineerAssignment.Api.Services;

public interface ICacheService
{
    T? Get<T>(object key);

    void Set(object key, object value, MemoryCacheEntryOptions? options = null);
}