using System.Reflection.Metadata;
using ThePlaylist.Core.Specification.Criterion;

namespace ThePlaylist.Core.Specification;



public class SpecificationBuilder<T>
{
    private readonly Specification<T> _specification;
    
    internal SpecificationBuilder()
    {
        this._specification = new Specification<T>();
    }

    public ISpecification<T> Build()
    {
        return _specification;
    }

    public SpecificationBuilder<T> Where(ICriteria restriction)
    {
        _specification.AddCriteria(restriction);
        return this;
    }
}