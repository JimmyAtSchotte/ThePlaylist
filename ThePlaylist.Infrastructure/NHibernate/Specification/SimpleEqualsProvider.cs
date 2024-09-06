using NHibernate;
using ThePlaylist.Core.Specification.Criterion;
using Restrictions = NHibernate.Criterion.Restrictions;

namespace ThePlaylist.Infrastructure.NHibernate.Specification;


public class SimpleEqualsFactory : ICriterionFactory
{
    public bool AppliesTo(ICriterion criterion) => criterion is SimpleCriterion { ComparisonType: ComparisonType.Equals };

    public ICriterionProvider Create(ICriterion criterion) => new SimpleEqualsProvider(criterion as SimpleCriterion);
}

public class SimpleEqualsProvider(SimpleCriterion? simpleCriterion) : ICriterionProvider
{
    public void ApplyTo(ICriteria criteria)
    {
        criteria.Add(Restrictions.Eq(simpleCriterion.Property, simpleCriterion.Value));
    }
}