namespace AzureIntelligentSupplyChain.Infrastructure.Persistence.MongoDb;

/// <summary>
/// MongoDB context for managing database connections and collections.
/// </summary>
public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IMongoClient mongoClient, string databaseName)
    {
        _database = mongoClient.GetDatabase(databaseName);
    }

    /// <summary>
    /// Gets a MongoDB collection by name.
    /// </summary>
    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }

    /// <summary>
    /// Gets the MongoDB database instance.
    /// </summary>
    public IMongoDatabase GetDatabase()
    {
        return _database;
    }

    /// <summary>
    /// Creates indexes for a collection.
    /// </summary>
    public async Task CreateIndexesAsync<T>(string collectionName, List<CreateIndexModel<T>> indexes)
    {
        var collection = GetCollection<T>(collectionName);
        await collection.Indexes.CreateManyAsync(indexes);
    }

    /// <summary>
    /// Drops a collection.
    /// </summary>
    public async Task DropCollectionAsync(string collectionName)
    {
        await _database.DropCollectionAsync(collectionName);
    }

    /// <summary>
    /// Checks if a collection exists.
    /// </summary>
    public async Task<bool> CollectionExistsAsync(string collectionName)
    {
        var filter = new BsonDocument("name", collectionName);
        var collections = await _database.ListCollectionsAsync(new ListCollectionsOptions { Filter = filter });
        return await collections.AnyAsync();
    }
}
