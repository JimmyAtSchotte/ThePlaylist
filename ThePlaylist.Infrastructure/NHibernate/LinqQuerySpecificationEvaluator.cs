using Ardalis.Specification;

namespace ThePlaylist.Infrastructure.NHibernate;

public class LinqQuerySpecificationEvaluator : ISpecificationEvaluator
{
    private List<IEvaluator> Evaluators { get; } = new List<IEvaluator>();
    
    public LinqQuerySpecificationEvaluator()
    {
        Evaluators.AddRange(new IEvaluator[]
        {
            WhereEvaluator.Instance,
            OrderEvaluator.Instance,
            PaginationEvaluator.Instance
        });
    }
    
    
    public IQueryable<TResult> GetQuery<T, TResult>(IQueryable<T> query, ISpecification<T, TResult> specification) where T : class
    {
        if (specification is null) throw new ArgumentNullException(nameof(specification));
        if (specification.Selector is null && specification.SelectorMany is null) throw new SelectorNotFoundException();
        if (specification.Selector is not null && specification.SelectorMany is not null) throw new ConcurrentSelectorsException();

        query = GetQuery(query, (ISpecification<T>)specification);

        return specification.Selector is not null
            ? query.Select(specification.Selector)
            : query.SelectMany(specification.SelectorMany!);
    }

    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification, bool evaluateCriteriaOnly = false) where T : class
    {
        if (specification is null) throw new ArgumentNullException(nameof(specification));

        var evaluators = evaluateCriteriaOnly ? Evaluators.Where(x => x.IsCriteriaEvaluator) : Evaluators;

        foreach (var evaluator in evaluators)
            query = evaluator.GetQuery(query, specification);
        
        return query;
    }
}