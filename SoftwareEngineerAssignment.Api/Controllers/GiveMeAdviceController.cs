using Microsoft.AspNetCore.Mvc;
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

    public GiveMeAdviceController(ILogger<GiveMeAdviceController> logger, IAdviceSlipService adviceSlipService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _adviceSlipService = adviceSlipService ?? throw new ArgumentNullException(nameof(adviceSlipService));
    }

    [HttpPost]
    public async Task<ActionResult<GiveMeAdviceResponse>> GiveMeAdvice(GiveMeAdviceRequest request)
    {
        _logger.LogInformation("GiveMeAdvice called with topic {topic} and amount {amount}", request.Topic, request.Amount);

        var adviceResponse = await _adviceSlipService.GetAdviceSlip(request);

        return adviceResponse.IsSuccessStatusCode
            ? Ok(adviceResponse)
            : Problem(
                statusCode: (int)adviceResponse.StatusCode,
                detail: $"Some error occurred while trying to reach {RouteConstants.AdviceSlipServiceBaseUrl}");
    }
}