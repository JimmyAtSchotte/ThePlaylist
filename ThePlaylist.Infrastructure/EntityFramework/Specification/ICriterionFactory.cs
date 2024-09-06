using ThePlaylist.Core.Specification.Criterion;

namespace ThePlaylist.Infrastructure.EntityFramework.Specification;

public interface ICriterionFactory
{
    bool AppliesTo(ICriterion criterion);

    ICriterionProvider Create(ICriterion criterion);
}