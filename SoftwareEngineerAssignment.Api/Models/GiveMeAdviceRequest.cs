using System.ComponentModel.DataAnnotations;

namespace SoftwareEngineerAssignment.Api.Models;

/// <summary>
/// This class implements IEquatable interface in order to be used as key in the cache service.
/// </summary>
public sealed class GiveMeAdviceRequest : IEquatable<GiveMeAdviceRequest>
{
    [Required]
    public string Topic { get; set; } = null!;

    [Range(0, int.MaxValue)]
    public int? Amount { get; set; }

    public bool Equals(GiveMeAdviceRequest? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Topic == other.Topic && Amount == other.Amount;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((GiveMeAdviceRequest)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Topic, Amount);
    }
}