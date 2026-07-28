namespace AzureIntelligentSupplyChain.Core.Domain.Common;

/// <summary>
/// Aggregate root base class for implementing DDD aggregate pattern.
/// </summary>
public abstract class AggregateRoot : Entity
{
    private readonly List<DomainEvent> _domainEvents = new();

    /// <summary>
    /// Gets the collection of uncommitted domain events.
    /// </summary>
    public IReadOnlyCollection<DomainEvent> GetDomainEvents()
    {
        return _domainEvents.AsReadOnly();
    }

    /// <summary>
    /// Adds a domain event to the aggregate.
    /// </summary>
    protected void AddDomainEvent(DomainEvent domainEvent)
    {
        domainEvent.AggregateId = Id;
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Clears all uncommitted domain events.
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
