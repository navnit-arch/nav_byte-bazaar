namespace AzureIntelligentSupplyChain.Core.Domain.Common;

/// <summary>
/// Domain event base class for implementing the domain event pattern.
/// </summary>
public abstract class DomainEvent
{
    /// <summary>
    /// Unique identifier for the domain event.
    /// </summary>
    public string Id { get; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Timestamp when the event occurred.
    /// </summary>
    public DateTime CreatedAt { get; } = DateTime.UtcNow;

    /// <summary>
    /// Event source or aggregate ID.
    /// </summary>
    public string? AggregateId { get; set; }

    /// <summary>
    /// Indicates if the event has been processed.
    /// </summary>
    public bool IsProcessed { get; set; }
}
