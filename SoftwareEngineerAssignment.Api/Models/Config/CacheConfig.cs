namespace SoftwareEngineerAssignment.Api.Models.Config;

public class CacheConfig
{
    public const string Key = "CacheConfig";
    public int AbsoluteExpirationInMinutes { get; set; }
}