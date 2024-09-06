using System.Linq.Expressions;
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

    public SpecificationBuilder<T> Where(ICriterion criterion)
    {
        _specification.AddCriterion(criterion);
        return this;
    }
    
    public SpecificationBuilder<T> Where(Expression<Func<T, bool>> expression)
    {
        _specification.Expression = expression;;
        return this;
    }
}