using ThePlaylist.Core.Specification.Criterion;

namespace ThePlaylist.Core.Specification;

public class Specification<T> : ISpecification<T>
{
    private readonly IList<ICriteria> _criteras = new List<ICriteria>();
    
    public void AddCriteria(ICriteria restriction)
    {
        _criteras.Add(restriction);
    }
    
    public IEnumerable<ICriteria> Criterias => _criteras;
}