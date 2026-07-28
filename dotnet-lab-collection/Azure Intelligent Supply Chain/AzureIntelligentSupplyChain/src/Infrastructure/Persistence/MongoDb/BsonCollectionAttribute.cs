namespace AzureIntelligentSupplyChain.Infrastructure.Persistence.MongoDb;

/// <summary>
/// Attribute to specify the MongoDB collection name for an entity.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class BsonCollectionAttribute : Attribute
{
    /// <summary>
    /// Collection name.
    /// </summary>
    public string CollectionName { get; }

    public BsonCollectionAttribute(string collectionName)
    {
        CollectionName = collectionName;
    }
}
