using NHibernate;
using ThePlaylist.Core.Specification.Criterion;

namespace ThePlaylist.Infrastructure.EntityFramework.Specification;

public class CriterionStrategy(ICriterionFactory[] factories)
{
    public ICriterionProvider? CreateProvider(ICriterion criterion)
    {
        return factories.FirstOrDefault(x => x.AppliesTo(criterion))?.Create(criterion);
    }
}