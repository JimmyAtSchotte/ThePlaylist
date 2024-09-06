using ThePlaylist.Core.Specification.Criterion;

namespace ThePlaylist.Infrastructure.NHibernate.Specification;

public interface ICriterionFactory
{
    bool AppliesTo(ICriterion criterion);

    ICriterionProvider Create(ICriterion criterion);
}