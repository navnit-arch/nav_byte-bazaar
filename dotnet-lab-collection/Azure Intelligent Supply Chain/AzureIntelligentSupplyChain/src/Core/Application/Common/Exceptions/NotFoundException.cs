namespace AzureIntelligentSupplyChain.Core.Application.Common.Exceptions;

/// <summary>
/// Exception thrown when a requested resource is not found.
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) 
        : base(message)
    {
    }

    public NotFoundException(string entity, string id) 
        : base($"{entity} with ID '{id}' was not found.")
    {
    }
}
