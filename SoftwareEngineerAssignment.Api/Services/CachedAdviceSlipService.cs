using Microsoft.Extensions.Caching.Memory;
using SoftwareEngineerAssignment.Api.Models;

namespace SoftwareEngineerAssignment.Api.Services;

public class CachedAdviceSlipService : IAdviceSlipService
{
    private readonly ILogger<CachedAdviceSlipService> _logger;
    private readonly ICacheService _cacheService;
    private readonly IAdviceSlipService _adviceSlipService;

    public CachedAdviceSlipService(ILogger<CachedAdviceSlipService> logger, ICacheService cacheService, IAdviceSlipService adviceSlipService)
    {
        _logger = logger;
        _cacheService = cacheService;
        _adviceSlipService = adviceSlipService;
    }

    public async Task<GiveMeAdviceResponse> GetAdviceSlip(GiveMeAdviceRequest request)
    {
        _logger.LogInformation("Cached advice-slip service called on topic {topic}", request.Topic);

        var cachedResponse = _cacheService.Get<GiveMeAdviceResponse>(request);
        if (cachedResponse != null)
        {
            _logger.LogInformation("Getting response from the cache");
            return cachedResponse;
        }

        var serviceResponse = await _adviceSlipService.GetAdviceSlip(request);
        if (serviceResponse.IsSuccessStatusCode)
        {
            _cacheService.Set(
                request,
                serviceResponse,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_cacheService.CacheConfig.AbsoluteExpirationInMinutes)
                });
        }

        return serviceResponse;
    }
}