using System.Linq.Expressions;
using ThePlaylist.Core.Specification.Criterion;

namespace ThePlaylist.Core.Specification;

public interface ISpecification<T>
{
    IEnumerable<ICriterion> Criteria { get; }
    
    Expression<Func<T, bool>>? Expression { get; set; }
}