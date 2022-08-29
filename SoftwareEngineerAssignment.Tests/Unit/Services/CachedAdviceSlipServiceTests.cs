using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using SoftwareEngineerAssignment.Api.Models;
using SoftwareEngineerAssignment.Api.Models.Config;
using SoftwareEngineerAssignment.Api.Services;

namespace SoftwareEngineerAssignment.Tests.Unit.Services;

public class CachedAdviceSlipServiceTests
{
    private readonly Mock<ILogger<CachedAdviceSlipService>> _mockedLogger;
    private readonly Mock<ICacheService> _mockedCacheService;
    private readonly Mock<IAdviceSlipService> _mockedAdviceSlipService;

    private CachedAdviceSlipService? _cachedAdviceSlipService;

    public CachedAdviceSlipServiceTests()
    {
        _mockedLogger = new Mock<ILogger<CachedAdviceSlipService>>();
        _mockedCacheService = new Mock<ICacheService>();
        _mockedAdviceSlipService = new Mock<IAdviceSlipService>();

        //_cachedAdviceSlipService = new CachedAdviceSlipService(logger.Object, cacheService.Object, adviceSlipService.Object);
    }

    [Fact]
    public async Task GetAdviceSlip_RequestNotCached_CallIAdviceServiceAndCacheResponse()
    {
        // Arrange
        var notCachedRequest = new GiveMeAdviceRequest
        {
            Topic = "not cached"
        };

        _mockedCacheService
            .Setup(x => x.Get<GiveMeAdviceResponse>(notCachedRequest))
            .Returns(value: null);
        _mockedCacheService
            .Setup(x => x.CacheConfig)
            .Returns(value: new CacheConfig() { AbsoluteExpirationInMinutes = 1 });

        _mockedAdviceSlipService
            .Setup(x => x.GetAdviceSlip(notCachedRequest))
            .ReturnsAsync(value: new GiveMeAdviceResponse() { IsSuccessStatusCode = true });

        _cachedAdviceSlipService = new CachedAdviceSlipService(_mockedLogger.Object, _mockedCacheService.Object,
            _mockedAdviceSlipService.Object);

        // Act
        var response = await _cachedAdviceSlipService.GetAdviceSlip(notCachedRequest);

        // Assert
        _mockedAdviceSlipService.Verify(x => x.GetAdviceSlip(notCachedRequest), Times.Once);
        _mockedCacheService.Verify(x => x.Set(notCachedRequest, It.IsAny<GiveMeAdviceResponse>(), It.IsAny<MemoryCacheEntryOptions>()), Times.Once);
    }

    [Fact]
    public async Task GetAdviceSlip_RequestCached_DoNotCallIAdviceServiceAndCacheResponse()
    {
        // Arrange
        var cachedRequest = new GiveMeAdviceRequest
        {
            Topic = "cached"
        };

        _mockedCacheService
            .Setup(x => x.Get<GiveMeAdviceResponse>(cachedRequest))
            .Returns(value: new GiveMeAdviceResponse() { IsSuccessStatusCode = true });
        _mockedCacheService
            .Setup(x => x.CacheConfig)
            .Returns(value: new CacheConfig() { AbsoluteExpirationInMinutes = 1 });

        _mockedAdviceSlipService
            .Setup(x => x.GetAdviceSlip(cachedRequest))
            .ReturnsAsync(value: new GiveMeAdviceResponse() { IsSuccessStatusCode = true });

        _cachedAdviceSlipService = new CachedAdviceSlipService(_mockedLogger.Object, _mockedCacheService.Object, _mockedAdviceSlipService.Object);

        // Act
        var response = await _cachedAdviceSlipService.GetAdviceSlip(cachedRequest);

        // Assert
        _mockedAdviceSlipService.Verify(x => x.GetAdviceSlip(cachedRequest), Times.Never);
        _mockedCacheService.Verify(x => x.Set(cachedRequest, It.IsAny<GiveMeAdviceResponse>(), It.IsAny<MemoryCacheEntryOptions>()), Times.Never);
    }
}