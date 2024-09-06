using NHibernate;
using ThePlaylist.Core.Specification.Criterion;

namespace ThePlaylist.Infrastructure.NHibernate.Specification;

public class CriterionStrategy(ICriterionFactory[] factories)
{
    public void Apply(ICriteria criteria, ICriterion criterion)
    {
        factories.FirstOrDefault(x => x.AppliesTo(criterion))?
            .Create(criterion)?
            .ApplyTo(criteria);
    }
}