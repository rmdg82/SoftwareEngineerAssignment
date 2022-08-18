using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SoftwareEngineerAssignment.Api.Constants;
using SoftwareEngineerAssignment.Api.Models;
using SoftwareEngineerAssignment.Api.Services;

namespace SoftwareEngineerAssignment.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GiveMeAdviceController : ControllerBase
{
    private readonly ILogger<GiveMeAdviceController> _logger;
    private readonly IAdviceSlipService _adviceSlipService;
    private readonly ICacheService _cache;

    public GiveMeAdviceController(ILogger<GiveMeAdviceController> logger, IAdviceSlipService adviceSlipService, ICacheService cache)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _adviceSlipService = adviceSlipService ?? throw new ArgumentNullException(nameof(adviceSlipService));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    [HttpPost]
    public async Task<ActionResult<GiveMeAdviceResponse>> GiveMeAdvice(GiveMeAdviceRequest request)
    {
        _logger.LogInformation("GiveMeAdvice called on topic {topic} and amount {amount}", request.Topic, request.Amount);

        var cachedResponse = _cache.Get<GiveMeAdviceResponse>(request);
        if (cachedResponse != null)
        {
            _logger.LogInformation("Getting response from the cache.");
            return Ok(cachedResponse);
        }

        var serviceResponse = await _adviceSlipService.GetAdviceSlip(request);
        if (!serviceResponse.IsSuccessStatusCode)
        {
            return Problem(
                statusCode: (int)serviceResponse.StatusCode,
                detail: $"Some error occurred while trying to reach {RouteConstants.AdviceSlipServiceBaseUrl}");
        }

        _cache.Set(request, serviceResponse, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) });

        return Ok(serviceResponse);
    }
}