using NHibernate;
using ThePlaylist.Core.Specification;

namespace ThePlaylist.Infrastructure.EntityFramework.Specification;

public class SpecificationQuery
{
    private readonly CriterionStrategy criterionStrategy = new(new[]
    {
        new SimpleEqualsFactory()
    });

    public IEnumerable<T> Execute<T>(IQueryable<T> queryable, ISpecification<T> specification)
        where T : class
    {
        if(specification.Expression is not null) 
            return queryable.Where(specification.Expression).ToList();
        
        foreach (var criterion in specification.Criteria)
            criterionStrategy.Apply(queryable, criterion);

        return queryable.ToList();;
    }
}