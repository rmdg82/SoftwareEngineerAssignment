using System.Text.Json.Serialization;

namespace SoftwareEngineerAssignment.Api.Models.AdviceSlip;

public class SearchResponse
{
    [JsonPropertyName("total_results")]
    public string TotalResult { get; set; } = null!;

    [JsonPropertyName("query")]
    public string Query { get; set; } = null!;

    [JsonPropertyName("slips")]
    public IEnumerable<Slip> Slips { get; set; } = new List<Slip>();
}