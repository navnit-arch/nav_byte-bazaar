namespace AzureIntelligentSupplyChain.Core.Domain.Common;

/// <summary>
/// Base specification class for building queries with DDD principles.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public abstract class Specification<TEntity> where TEntity : Entity
{
    public Expression<Func<TEntity, bool>>? Criteria { get; protected set; }
    public List<Expression<Func<TEntity, object>>> Includes { get; } = new();
    public List<string> IncludeStrings { get; } = new();
    public Expression<Func<TEntity, object>>? OrderBy { get; protected set; }
    public Expression<Func<TEntity, object>>? OrderByDescending { get; protected set; }
    public int? Take { get; protected set; }
    public int? Skip { get; protected set; }
    public bool IsPagingEnabled { get; protected set; }

    protected virtual void AddInclude(Expression<Func<TEntity, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected virtual void AddInclude(string includeString)
    {
        IncludeStrings.Add(includeString);
    }
}

