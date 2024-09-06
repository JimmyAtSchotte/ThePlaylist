using NHibernate;
using ThePlaylist.Core.Specification;
using ThePlaylist.Core.Specification.Criterion;
using Restrictions = NHibernate.Criterion.Restrictions;

namespace ThePlaylist.Infrastructure.NHibernate;


public static class SessionExtension
{
    private static readonly SpecificationQuery SpecificationQuery = new SpecificationQuery();
    
    public static IEnumerable<T> Query<T>(this ISession session, ISpecification<T> specification)
        where T : class
    {
        return SpecificationQuery.Execute(session, specification);
    }
}


public class SpecificationQuery
{
    private readonly CriterionStrategy criterionStrategy;

    public SpecificationQuery()
    {
        criterionStrategy = new CriterionStrategy(new[]
        {
            new EqualsFactory()
        });
    }
    
    public IEnumerable<T> Execute<T>(ISession session, ISpecification<T> specification)
        where T : class
    {
        var query = session.CreateCriteria<T>();
        
        foreach (var criteria in specification.Criterias)
            criterionStrategy.Apply(query, criteria);

        return query.List<T>().ToList();;
    }
}

public class CriterionStrategy(ICriterionFactory[] factories)
{
    public void Apply(ICriteria criteria, ICriterion criterion)
    {
        factories.FirstOrDefault(x => x.AppliesTo(criterion))?
            .Create(criterion)?
            .ApplyTo(criteria);
    }
}

public interface ICriterionFactory
{
    bool AppliesTo(ICriterion criterion);

    ICriterionProvider Create(ICriterion criterion);
}

public interface ICriterionProvider
{
    void ApplyTo(ICriteria criteria);
}

public class EqualsProvider(SimpleCriterion? simpleCriterion) : ICriterionProvider
{
    public void ApplyTo(ICriteria criteria)
    {
        criteria.Add(Restrictions.Eq(simpleCriterion.Property, simpleCriterion.Value));
    }
}

public class EqualsFactory : ICriterionFactory
{
    public bool AppliesTo(ICriterion criterion) => criterion is SimpleCriterion { ComparisonType: ComparisonType.Equals };

    public ICriterionProvider Create(ICriterion criterion) => new EqualsProvider(criterion as SimpleCriterion);
}

