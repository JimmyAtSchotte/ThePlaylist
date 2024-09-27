using Ardalis.Specification;
using ThePlaylist.Core.Entitites;

namespace ThePlaylist.Specifications;

public static class Specs
{
    public static SpecificationSet<Playlist> Playlist => new SpecificationSet<Playlist>();
    public static SpecificationSet<Track> Track => new SpecificationSet<Track>();
    public static SpecificationSet<Genre> Genre => new SpecificationSet<Genre>();
    
    public static ISpecification<T> ById<T>(Guid id) 
        where T : IEntity => new ById<T>(id);
    
    public static ISpecification<T> ById<T>(Guid id, Action<ISpecificationBuilder<T>> specification) 
        where T : IEntity => new ById<T>(id, specification);
}

public class SpecificationSet<T>
{
    
}

