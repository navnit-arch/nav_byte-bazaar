namespace AzureIntelligentSupplyChain.Core.Domain.Common;

/// <summary>
/// Base entity class for all domain entities following DDD principles.
/// </summary>
public abstract class Entity
{
    /// <summary>
    /// Unique identifier for the entity.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// Timestamp when the entity was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// User who created the entity.
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Timestamp when the entity was last modified.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// User who last modified the entity.
    /// </summary>
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// Indicates whether the entity is deleted (soft delete).
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Timestamp when the entity was deleted.
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>
    /// Version number for optimistic concurrency control.
    /// </summary>
    public int Version { get; set; }

    protected Entity()
    {
        Id = Guid.NewGuid().ToString();
        CreatedAt = DateTime.UtcNow;
        Version = 1;
        IsDeleted = false;
    }

    /// <summary>
    /// Determines if two entities are equal by their ID.
    /// </summary>
    public override bool Equals(object? obj)
    {
        return obj is Entity entity && Id == entity.Id;
    }

    /// <summary>
    /// Gets hash code based on the entity's ID.
    /// </summary>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString()
    {
        return $"{GetType().Name} [Id={Id}]";
    }
}
