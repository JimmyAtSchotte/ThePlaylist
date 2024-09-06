using ThePlaylist.Core.Specification.Criterion;

namespace ThePlaylist.Core.Specification;

public interface ISpecification<T>
{
    IEnumerable<ICriterion> Criterias { get; }
}