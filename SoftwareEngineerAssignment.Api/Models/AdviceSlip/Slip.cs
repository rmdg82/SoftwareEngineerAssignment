using System.Text.Json.Serialization;

namespace SoftwareEngineerAssignment.Api.Models.AdviceSlip;

public class Slip
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("advice")]
    public string Advice { get; set; } = null!;

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }
}