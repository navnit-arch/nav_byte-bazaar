namespace AzureIntelligentSupplyChain.Core.Application.Common.Exceptions;

/// <summary>
/// Exception thrown when a business rule is violated.
/// </summary>
public class BusinessRuleException : Exception
{
    /// <summary>
    /// The error code.
    /// </summary>
    public string ErrorCode { get; set; }

    public BusinessRuleException(string message, string errorCode = "BUSINESS_RULE_VIOLATION") 
        : base(message)
    {
        ErrorCode = errorCode;
    }
}
