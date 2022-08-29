using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SoftwareEngineerAssignment.Api.Controllers;
using SoftwareEngineerAssignment.Api.Models;
using SoftwareEngineerAssignment.Api.Services;

namespace SoftwareEngineerAssignment.Tests.Unit.Controllers;

public class GiveMeAdviceControllerTests
{
    private readonly Mock<IAdviceSlipService> _mockAdviceSlipService;
    private readonly Mock<ILogger<GiveMeAdviceController>> _mockLogger;
    private readonly Mock<ICacheService> _mockCache;

    private GiveMeAdviceController? _controller;

    public GiveMeAdviceControllerTests()
    {
        _mockLogger = new Mock<ILogger<GiveMeAdviceController>>();
        _mockAdviceSlipService = new Mock<IAdviceSlipService>();
        _mockCache = new Mock<ICacheService>();
    }

    [Fact]
    public void GiveMeAdvice_RequestObjectNotValid_ReturnBadRequest()
    {
        // Arrange
        var incorrectRequest = new GiveMeAdviceRequest()
        {
            Topic = "test",
            Amount = -1
        };

        // Act
        var validationResult = new List<ValidationResult>();
        var context = new ValidationContext(incorrectRequest, null, null);
        Validator.TryValidateObject(incorrectRequest, context, validationResult, true);

        // Assert
        Assert.True(validationResult.Count > 0);
    }

    [Fact]
    public void GiveMeAdvice_RequestObjectValid_ReturnOk()
    {
        // Arrange
        var correctRequest = new GiveMeAdviceRequest()
        {
            Topic = "test",
            Amount = 1
        };

        // Act
        var validationResult = new List<ValidationResult>();
        var context = new ValidationContext(correctRequest, null, null);
        Validator.TryValidateObject(correctRequest, context, validationResult, true);

        // Assert
        Assert.True(validationResult.Count == 0);
    }

    [Fact]
    public async Task GiveMeAdvice_WhenCalledWithNoSuccess_ReturnProblem()
    {
        // Arrange
        var correctRequest = new GiveMeAdviceRequest()
        {
            Topic = "test",
            Amount = 2
        };

        _mockAdviceSlipService
            .Setup(x => x.GetAdviceSlip(correctRequest))
            .ReturnsAsync(new GiveMeAdviceResponse()
            {
                AdviceList = Enumerable.Empty<string>(),
                IsSuccessStatusCode = false,
                StatusCode = HttpStatusCode.BadRequest
            });

        _controller = new GiveMeAdviceController(_mockLogger.Object, _mockAdviceSlipService.Object);

        // Act
        var sut = await _controller.GiveMeAdvice(correctRequest);

        // Assert
        var actionResult = Assert.IsType<ActionResult<GiveMeAdviceResponse>>(sut);
        Assert.NotNull(actionResult);

        var result = Assert.IsType<ObjectResult>(actionResult.Result);
        Assert.NotNull(result);

        var problem = Assert.IsType<ProblemDetails>(result.Value);
        Assert.NotNull(problem);
        Assert.NotNull(problem.Status);
        Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)problem.Status);
    }

    [Fact]
    public async Task GiveMeAdvice_WhenCalledWithSuccess_ReturnOk()
    {
        // Arrange
        var correctRequest = new GiveMeAdviceRequest()
        {
            Topic = "test",
            Amount = 2
        };

        _mockAdviceSlipService
            .Setup(x => x.GetAdviceSlip(correctRequest))
            .ReturnsAsync(new GiveMeAdviceResponse()
            {
                AdviceList = Enumerable.Empty<string>(),
                IsSuccessStatusCode = true,
                StatusCode = HttpStatusCode.OK
            });

        _controller = new GiveMeAdviceController(_mockLogger.Object, _mockAdviceSlipService.Object);

        // Act
        var sut = await _controller.GiveMeAdvice(correctRequest);

        // Assert
        var actionResult = Assert.IsType<ActionResult<GiveMeAdviceResponse>>(sut);
        Assert.NotNull(actionResult);

        var result = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.NotNull(result);

        var giveMeAdviceResponse = Assert.IsType<GiveMeAdviceResponse>(result.Value);
        Assert.NotNull(giveMeAdviceResponse);

        Assert.Equal(HttpStatusCode.OK, giveMeAdviceResponse.StatusCode);
    }

    // TODO: write integration tests
    // TODO: write DI tests

    //[Fact]
    //public async Task GiveMeAdvice_WhenCalledNoCachedElement_SetTheCache()
    //{
    //    // Arrange
    //    var noCachedRequest = new GiveMeAdviceRequest()
    //    {
    //        Topic = "no-cached",
    //        Amount = 1
    //    };

    //    _mockCache
    //        .Setup(x => x.Get<GiveMeAdviceResponse>(noCachedRequest))
    //        .Returns(value: null);

    //    //_mockAdviceSlipService
    //    //    .Setup(x => x.GetAdviceSlip(correctRequest))
    //    //    .ReturnsAsync(new GiveMeAdviceResponse()
    //    //    {
    //    //        AdviceList = new List<string> { "advice1", "advice2" },
    //    //        IsSuccessStatusCode = true,
    //    //        StatusCode = HttpStatusCode.OK
    //    //    });

    //    _controller = new GiveMeAdviceController(_mockLogger.Object, _mockAdviceSlipService.Object);

    //    // Act
    //    var sut = await _controller.GiveMeAdvice(noCachedRequest);

    //    // Assert
    //    var actionResult = Assert.IsType<ActionResult<GiveMeAdviceResponse>>(sut);
    //    Assert.NotNull(actionResult);

    //    var result = Assert.IsType<OkObjectResult>(actionResult.Result);
    //    Assert.NotNull(result);

    //    var giveMeAdviceResponse = Assert.IsType<GiveMeAdviceResponse>(result.Value);
    //    Assert.NotNull(giveMeAdviceResponse);

    //    var advices = Assert.IsAssignableFrom<IEnumerable<string>>(giveMeAdviceResponse.AdviceList);
    //    Assert.Equal(2, advices.Count());

    //    _mockCache.Verify(x => x.Set(noCachedRequest, giveMeAdviceResponse, It.IsAny<MemoryCacheEntryOptions>()), Times.Once);
    //}

    //[Fact]
    //public async Task GiveMeAdvice_WhenCalledWithCache_GetElementFromTheCache()
    //{
    //    // Arrange
    //    var cachedRequest = new GiveMeAdviceRequest()
    //    {
    //        Topic = "test",
    //        Amount = 2
    //    };

    //    _mockCache
    //        .Setup(x => x.Get<GiveMeAdviceResponse>(cachedRequest))
    //        .Returns(new GiveMeAdviceResponse()
    //        {
    //            AdviceList = new List<string> { "advice1", "advice2" },
    //            IsSuccessStatusCode = true,
    //            StatusCode = HttpStatusCode.OK
    //        });

    //    _controller = new GiveMeAdviceController(_mockLogger.Object, _mockAdviceSlipService.Object);

    //    // Act
    //    var sut = await _controller.GiveMeAdvice(cachedRequest);

    //    // Assert
    //    var actionResult = Assert.IsType<ActionResult<GiveMeAdviceResponse>>(sut);
    //    Assert.NotNull(actionResult);

    //    var result = Assert.IsType<OkObjectResult>(actionResult.Result);
    //    Assert.NotNull(result);

    //    var giveMeAdviceResponse = Assert.IsType<GiveMeAdviceResponse>(result.Value);
    //    Assert.NotNull(giveMeAdviceResponse);

    //    var advices = Assert.IsAssignableFrom<IEnumerable<string>>(giveMeAdviceResponse.AdviceList);
    //    Assert.Equal(2, advices.Count());

    //    _mockCache.Verify(x => x.Get<GiveMeAdviceResponse>(cachedRequest), Times.Once);
    //}
}