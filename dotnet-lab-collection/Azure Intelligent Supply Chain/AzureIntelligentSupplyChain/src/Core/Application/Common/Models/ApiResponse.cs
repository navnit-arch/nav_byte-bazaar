namespace AzureIntelligentSupplyChain.Core.Application.Common.Models;

/// <summary>
/// Standard API response wrapper.
/// </summary>
/// <typeparam name="T">The data type.</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Indicates if the operation was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Response message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Response data.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// List of validation errors.
    /// </summary>
    public Dictionary<string, string[]>? Errors { get; set; }

    /// <summary>
    /// Request ID for tracing.
    /// </summary>
    public string? TraceId { get; set; }

    public ApiResponse()
    {
    }

    public ApiResponse(T? data, string message = "", bool success = true)
    {
        Data = data;
        Message = message;
        Success = success;
    }

    public static ApiResponse<T> SuccessResponse(T? data, string message = "Operation successful")
    {
        return new ApiResponse<T> { Success = true, Data = data, Message = message };
    }

    public static ApiResponse<T> FailureResponse(string message, Dictionary<string, string[]>? errors = null)
    {
        return new ApiResponse<T> { Success = false, Message = message, Errors = errors };
    }
}

/// <summary>
/// Standard API response wrapper for non-generic responses.
/// </summary>
public class ApiResponse
{
    /// <summary>
    /// Indicates if the operation was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Response message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// List of validation errors.
    /// </summary>
    public Dictionary<string, string[]>? Errors { get; set; }

    /// <summary>
    /// Request ID for tracing.
    /// </summary>
    public string? TraceId { get; set; }

    public ApiResponse()
    {
    }

    public ApiResponse(string message, bool success = true)
    {
        Message = message;
        Success = success;
    }

    public static ApiResponse SuccessResponse(string message = "Operation successful")
    {
        return new ApiResponse { Success = true, Message = message };
    }

    public static ApiResponse FailureResponse(string message, Dictionary<string, string[]>? errors = null)
    {
        return new ApiResponse { Success = false, Message = message, Errors = errors };
    }
}
