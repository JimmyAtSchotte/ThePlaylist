using Ardalis.Specification;
using ThePlaylist.Core.Specification;

namespace ThePlaylist.Infrastructure.EntityFramework.Specification;

public class CriteraEvaluator: IEvaluator
{
    private readonly CriterionStrategy _criterionStrategy;
    
    public static CriteraEvaluator Instance { get; } = new CriteraEvaluator();

    private CriteraEvaluator()
    {
        _criterionStrategy = new CriterionStrategy([
            new SimpleEqualsFactory()
        ]);
    }
    
    
    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        var criteria = specification.Criteria();

        foreach (var criterion in criteria)
        {
            var provider = _criterionStrategy.CreateProvider(criterion);
            var expression = provider?.CreateExpression<T>();
            
            query = query.Where(expression);
        }

        return query;
    }

    public bool IsCriteriaEvaluator { get; } = true;
}