using System.Net;
using System.Text.Json.Serialization;

namespace SoftwareEngineerAssignment.Api.Models;

public class GiveMeAdviceResponse

{
    [JsonPropertyName("adviceList")]
    public IEnumerable<string> AdviceList { get; set; } = new List<string>();

    [JsonIgnore]
    public bool IsSuccessStatusCode { get; set; } = true;

    [JsonIgnore]
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
}