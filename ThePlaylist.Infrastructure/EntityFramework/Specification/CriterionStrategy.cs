using NHibernate;
using ThePlaylist.Core.Specification.Criterion;

namespace ThePlaylist.Infrastructure.EntityFramework.Specification;

public class CriterionStrategy(ICriterionFactory[] factories)
{
    public void Apply<T>(IQueryable<T> queryable, ICriterion criterion)
    {
        factories.FirstOrDefault(x => x.AppliesTo(criterion))?
            .Create(criterion)?
            .ApplyTo(queryable);
    }
}