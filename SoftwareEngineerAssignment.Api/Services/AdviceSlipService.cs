using SoftwareEngineerAssignment.Api.Models;
using System.Text.Json;
using SoftwareEngineerAssignment.Api.Constants;
using SoftwareEngineerAssignment.Api.Models.AdviceSlip;

namespace SoftwareEngineerAssignment.Api.Services;

public class AdviceSlipService : IAdviceSlipService
{
    private readonly HttpClient _httpClient;

    public AdviceSlipService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<GiveMeAdviceResponse> GetAdviceSlip(GiveMeAdviceRequest request)
    {
        var response = await _httpClient.GetAsync(RouteConstants.AdviceSlipServiceSearchUrl + request.Topic);

        if (!response.IsSuccessStatusCode)
        {
            return new GiveMeAdviceResponse()
            {
                IsSuccessStatusCode = false,
                StatusCode = response.StatusCode
            };
        }

        var content = await response.Content.ReadAsStringAsync();

        var advices = JsonSerializer.Deserialize<SearchResponse>(content);

        if (advices is null || !advices.Slips.Any())
        {
            return new GiveMeAdviceResponse();
        }

        var advicesToKeep = request.Amount.HasValue ? advices!.Slips.Take(request.Amount.Value).ToList() : advices!.Slips.ToList();

        return new GiveMeAdviceResponse()
        {
            AdviceList = advicesToKeep.Select(a => a.Advice)
        };
    }
}