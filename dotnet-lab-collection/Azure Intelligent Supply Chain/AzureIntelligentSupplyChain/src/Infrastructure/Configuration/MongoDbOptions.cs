namespace AzureIntelligentSupplyChain.Infrastructure.Configuration;

/// <summary>
/// MongoDB configuration options.
/// </summary>
public class MongoDbOptions
{
    public const string SectionName = "MongoDb";

    /// <summary>
    /// MongoDB connection string.
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Database name.
    /// </summary>
    public string DatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if the database should be initialized on startup.
    /// </summary>
    public bool InitializeOnStartup { get; set; } = true;
}
