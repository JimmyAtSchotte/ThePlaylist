using System.Reflection.Metadata;

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

    public SpecificationBuilder<T> Where(Restriction restriction)
    {
        _specification.AddCriteria(restriction);
        return this;
    }
}

public interface ISpecification<T>
{
    IEnumerable<Restriction> Criterias { get; }
}

public class Specification<T> : ISpecification<T>
{
    private readonly IList<Restriction> _criteras = new List<Restriction>();
    
    public void AddCriteria(Restriction restriction)
    {
        _criteras.Add(restriction);
    }
    
    public IEnumerable<Restriction> Criterias => _criteras;
}

public enum RestrictionType
{
    Equals
}

public class Restrictions
{
    public static Restriction Eq(string property, object value) => new Restriction(property, RestrictionType.Equals, value);
}

public class Restriction(string property, RestrictionType restrictionType, object value)
{
    public string Property { get; } = property;
    public RestrictionType RestrictionType { get; } = restrictionType;
    public object Value { get; } = value;
}