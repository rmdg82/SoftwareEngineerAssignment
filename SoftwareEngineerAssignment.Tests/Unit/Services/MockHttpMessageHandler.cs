using System.Net;
using System.Text.Json;
using SoftwareEngineerAssignment.Api.Models.AdviceSlip;

namespace SoftwareEngineerAssignment.Tests.Unit.Services;

/// <summary>
/// Mocked message handler for testing purposes. Used to mock the HttpClient.
/// </summary>
public class MockHttpMessageHandler : HttpMessageHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var searchTopic = request.RequestUri!.Segments.Last();

        return searchTopic switch
        {
            "apiIsNotReachable" => new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.ServiceUnavailable
            },

            "topicWithNoAdvice" => new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Accepted,
                Content = new StringContent(JsonSerializer.Serialize(new SearchResponse()
                {
                    Slips = Enumerable.Empty<Slip>()
                }))
            },

            "topicWithThreeAdvice" => new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Accepted,
                Content = new StringContent(JsonSerializer.Serialize(new SearchResponse()
                {
                    Slips = new List<Slip>
                    {
                        new()
                        {
                            Advice = "advice1"
                        },
                        new()
                        {
                            Advice = "advice2"
                        },
                        new()
                        {
                            Advice = "advice3"
                        }
                    },
                    Query = "topicWithThreeAdvice",
                    TotalResult = "3"
                }))
            },

            _ => throw new NotImplementedException()
        };
    }
}