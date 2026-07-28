namespace AzureIntelligentSupplyChain.Core.Domain.Common;

/// <summary>
/// Base value object class following DDD principles.
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// Gets the equality components for value object comparison.
    /// </summary>
    protected abstract IEnumerable<object?> GetEqualityComponents();

    /// <summary>
    /// Determines equality based on all components.
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;

        var valueObject = (ValueObject)obj;
        return GetEqualityComponents()
            .SequenceEqual(valueObject.GetEqualityComponents());
    }

    /// <summary>
    /// Gets hash code based on all components.
    /// </summary>
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }

    public override string ToString()
    {
        return GetType().Name;
    }
}
