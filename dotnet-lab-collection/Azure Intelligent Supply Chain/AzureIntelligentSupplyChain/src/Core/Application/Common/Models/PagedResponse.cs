namespace AzureIntelligentSupplyChain.Core.Application.Common.Models;

/// <summary>
/// Paginated response wrapper.
/// </summary>
/// <typeparam name="T">The item type.</typeparam>
public class PagedResponse<T>
{
    /// <summary>
    /// Current page number.
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Page size.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total number of items.
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Total number of pages.
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Items in the current page.
    /// </summary>
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();

    /// <summary>
    /// Indicates if there's a next page.
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;

    /// <summary>
    /// Indicates if there's a previous page.
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;

    public PagedResponse()
    {
    }

    public PagedResponse(IEnumerable<T> items, int pageNumber, int pageSize, int totalItems)
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalItems = totalItems;
        TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
    }

    public static PagedResponse<T> Create(IEnumerable<T> items, int pageNumber, int pageSize, int totalItems)
    {
        return new PagedResponse<T>(items, pageNumber, pageSize, totalItems);
    }
}
