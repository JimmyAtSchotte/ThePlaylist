using NHibernate;
using ThePlaylist.Core.Specification;

namespace ThePlaylist.Infrastructure.NHibernate.Specification;

public class SpecificationQuery
{
    private readonly CriterionStrategy criterionStrategy = new(new[]
    {
        new SimpleEqualsFactory()
    });

    public IEnumerable<T> Execute<T>(ISession session, ISpecification<T> specification)
        where T : class
    {
        if(specification.Expression is not null) 
            return session.Query<T>().Where(specification.Expression).ToList();
        
        var criteria = session.CreateCriteria<T>();
        
        foreach (var criterion in specification.Criteria)
            criterionStrategy.Apply(criteria, criterion);

        return criteria.List<T>().ToList();;
    }
}