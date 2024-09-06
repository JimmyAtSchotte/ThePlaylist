using System.Linq.Expressions;
using ThePlaylist.Core.Specification.Criterion;

namespace ThePlaylist.Core.Specification;

public class Specification<T> : ISpecification<T>
{
    private readonly IList<ICriterion> _criteria = new List<ICriterion>();
    
    public void AddCriterion(ICriterion restriction)
    {
        _criteria.Add(restriction);
    }
    
    public IEnumerable<ICriterion> Criteria => _criteria;
    public Expression<Func<T, bool>> Expression { get; set; }
}