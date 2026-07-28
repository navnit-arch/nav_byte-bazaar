namespace AzureIntelligentSupplyChain.Core.Application.Common.Exceptions;

/// <summary>
/// Exception thrown when a validation error occurs.
/// </summary>
public class ValidationException : Exception
{
    /// <summary>
    /// Dictionary of validation errors.
    /// </summary>
    public Dictionary<string, string[]> Errors { get; set; }

    public ValidationException(string message = "Validation failed", 
        Dictionary<string, string[]>? errors = null) 
        : base(message)
    {
        Errors = errors ?? new Dictionary<string, string[]>();
    }

    public ValidationException(Dictionary<string, string[]> errors) 
        : base("Validation failed")
    {
        Errors = errors;
    }
}
