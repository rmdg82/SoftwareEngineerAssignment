using System.Net;
using Microsoft.Extensions.Logging;
using Moq;
using SoftwareEngineerAssignment.Api.Constants;
using SoftwareEngineerAssignment.Api.Models;
using SoftwareEngineerAssignment.Api.Services;

namespace SoftwareEngineerAssignment.Tests.Unit.Services;

public class AdviceSlipServiceTests
{
    private readonly AdviceSlipService _adviceSplitService;

    public AdviceSlipServiceTests()
    {
        var mockedMessageHandler = new MockHttpMessageHandler();
        var httpClient = new HttpClient(mockedMessageHandler)
        {
            BaseAddress = new Uri(RouteConstants.AdviceSlipServiceBaseUrl)
        };

        var logger = new Mock<ILogger<AdviceSlipService>>();

        _adviceSplitService = new AdviceSlipService(logger.Object, httpClient);
    }

    [Fact]
    public async Task GetAdviceSlip_ApiIsNotReachable_ReturnsServiceUnavailable()
    {
        var sut = await _adviceSplitService.GetAdviceSlip(new GiveMeAdviceRequest() { Topic = "apiIsNotReachable" });

        Assert.Equal(HttpStatusCode.ServiceUnavailable, sut.StatusCode);
    }

    [Fact]
    public async Task GetAdviceSlip_GetNoAdvice_ReturnsEmptyAdviceList()
    {
        var sut = await _adviceSplitService.GetAdviceSlip(new GiveMeAdviceRequest() { Topic = "topicWithNoAdvice" });

        Assert.Empty(sut.AdviceList);
    }

    [Fact]
    public async Task GetAdviceSlip_AmountIsZero_ReturnsEmptyAdviceList()
    {
        var sut = await _adviceSplitService.GetAdviceSlip(new GiveMeAdviceRequest() { Topic = "topicWithThreeAdvice", Amount = 0 });

        Assert.Empty(sut.AdviceList);
    }

    [Fact]
    public async Task GetAdviceSlip_GetAdvicesAndAmountIsEmpty_ReturnsAllAdvices()
    {
        var sut = await _adviceSplitService.GetAdviceSlip(new GiveMeAdviceRequest() { Topic = "topicWithThreeAdvice" });

        Assert.Equal(3, sut.AdviceList.Count());
    }

    [Fact]
    public async Task GetAdviceSlip_GetManyAdvicesButAmountIsLessThanTheAdviceReceived_ReturnsExactlyTheAmount()
    {
        var sut = await _adviceSplitService.GetAdviceSlip(new GiveMeAdviceRequest() { Topic = "topicWithThreeAdvice", Amount = 2 });

        Assert.Equal(2, sut.AdviceList.Count());
    }
}