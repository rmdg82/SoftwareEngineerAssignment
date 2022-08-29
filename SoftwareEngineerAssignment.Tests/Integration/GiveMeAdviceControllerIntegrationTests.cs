using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using SoftwareEngineerAssignment.Api.Models;

namespace SoftwareEngineerAssignment.Tests.Integration;

public class GiveMeAdviceControllerIntegrationTests : IClassFixture<TestingApiWebAppFactory<Program>>
{
    private readonly HttpClient _httpClient;

    private readonly GiveMeAdviceRequest _request = new() { Topic = "car", Amount = 3 };

    public GiveMeAdviceControllerIntegrationTests(TestingApiWebAppFactory<Program> factory)
    {
        _httpClient = factory.CreateDefaultClient();
    }

    [Fact]
    public async Task GiveMeAdvice_WhenCalledOnCorrectAddress_ExecuteRequestWithSuccess()
    {
        var httpContent = JsonContent.Create(_request);

        var response = await _httpClient.PostAsync("api/GiveMeAdvice", httpContent);

        Assert.True(response.IsSuccessStatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        var advices = JsonSerializer.Deserialize<GiveMeAdviceResponse>(responseContent);

        Assert.NotNull(advices);
        Assert.IsType<GiveMeAdviceResponse>(advices);

        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.OK, advices.StatusCode);
    }

    [Fact]
    public async Task GiveMeAdvice_WhenCalledOnWrongAddress_ReturnsNotFound()
    {
        var httpContent = JsonContent.Create(_request);

        var response = await _httpClient.PostAsync("wrong/Path", httpContent);

        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}