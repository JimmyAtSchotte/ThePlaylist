using Ardalis.Specification;
using ThePlaylist.Core.Entitites;

namespace ThePlaylist.Specifications;

public sealed class ById<T> : Specification<T> where T : IEntity
{
    public ById(Guid id)
    {
        Query.Where(x => x.Id == id);
    }
    
    public ById(Guid id, Action<ISpecificationBuilder<T>> specification) : this(id)
    {
        specification.Invoke(Query);
    }
}