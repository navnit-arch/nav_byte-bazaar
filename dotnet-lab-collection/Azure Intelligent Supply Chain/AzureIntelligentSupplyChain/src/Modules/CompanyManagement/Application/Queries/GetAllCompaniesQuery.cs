namespace AzureIntelligentSupplyChain.Modules.CompanyManagement.Application.Queries;

/// <summary>
/// Query to get all companies with pagination.
/// </summary>
public class GetAllCompaniesQuery : IRequest<ApiResponse<PagedResponse<CompanyDto>>>
{
    /// <summary>
    /// Page number (1-based).
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Page size.
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Company status filter (optional).
    /// </summary>
    public int? StatusFilter { get; set; }

    /// <summary>
    /// Company type filter (optional).
    /// </summary>
    public int? CompanyTypeFilter { get; set; }

    /// <summary>
    /// Search term for name or registration number.
    /// </summary>
    public string? SearchTerm { get; set; }
}

/// <summary>
/// Specification for filtering companies.
/// </summary>
public class CompanySpecification : Specification<Company>
{
    public CompanySpecification(
        int pageNumber,
        int pageSize,
        int? statusFilter = null,
        int? companyTypeFilter = null,
        string? searchTerm = null)
    {
        var filters = new List<Expression<Func<Company, bool>>>();

        if (statusFilter.HasValue)
        {
            filters.Add(x => x.Status == (CompanyStatus)statusFilter.Value);
        }

        if (companyTypeFilter.HasValue)
        {
            filters.Add(x => x.CompanyType == (CompanyType)companyTypeFilter.Value);
        }

        if (!string.IsNullOrEmpty(searchTerm))
        {
            filters.Add(x => x.Name.Contains(searchTerm) || x.RegistrationNumber.Contains(searchTerm));
        }

        if (filters.Count > 0)
        {
            Criteria = filters.Aggregate((current, next) =>
                Expression.Lambda<Func<Company, bool>>(
                    Expression.AndAlso(current.Body, Expression.Invoke(next, current.Parameters[0])),
                    current.Parameters[0]));
        }

        OrderBy = x => x.CreatedAt;
        Skip = (pageNumber - 1) * pageSize;
        Take = pageSize;
        IsPagingEnabled = true;
    }
}

/// <summary>
/// Handler for getting all companies.
/// </summary>
public class GetAllCompaniesQueryHandler : IRequestHandler<GetAllCompaniesQuery, ApiResponse<PagedResponse<CompanyDto>>>
{
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllCompaniesQueryHandler> _logger;

    public GetAllCompaniesQueryHandler(
        IRepositoryFactory repositoryFactory,
        IMapper mapper,
        ILogger<GetAllCompaniesQueryHandler> logger)
    {
        _repositoryFactory = repositoryFactory;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<PagedResponse<CompanyDto>>> Handle(
        GetAllCompaniesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Getting all companies - Page: {PageNumber}, Size: {PageSize}", 
                request.PageNumber, request.PageSize);

            var repository = _repositoryFactory.CreateRepository<Company>();

            var specification = new CompanySpecification(
                request.PageNumber,
                request.PageSize,
                request.StatusFilter,
                request.CompanyTypeFilter,
                request.SearchTerm);

            var companies = await repository.GetBySpecificationAsync(specification, cancellationToken);
            var totalCount = await repository.CountAsync(
                new CompanySpecification(
                    1,
                    int.MaxValue,
                    request.StatusFilter,
                    request.CompanyTypeFilter,
                    request.SearchTerm),
                cancellationToken);

            var dtos = _mapper.Map<List<CompanyDto>>(companies);
            var pagedResponse = PagedResponse<CompanyDto>.Create(
                dtos,
                request.PageNumber,
                request.PageSize,
                totalCount);

            return ApiResponse<PagedResponse<CompanyDto>>.SuccessResponse(pagedResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all companies");
            return ApiResponse<PagedResponse<CompanyDto>>.FailureResponse(
                "An error occurred while retrieving companies");
        }
    }
}
