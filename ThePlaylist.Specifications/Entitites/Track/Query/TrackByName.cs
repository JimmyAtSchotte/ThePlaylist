using System.Linq.Expressions;
using Ardalis.Specification;

namespace ThePlaylist.Specifications.Entitites.Track.Query;


public static partial class SpecificationSetExtensions
{
    private static Expression<Func<Core.Entitites.Track, bool>> Predicate(string name)
    {
        return x => x.Name == name;
    }
    
    public static Specification<Core.Entitites.Track> TrackByName(
        this SpecificationSet<Core.Entitites.Track> _, string name)
        => new TrackByName(Predicate(name));
    
    public static Specification<Core.Entitites.Track, TProjection> TrackByName<TProjection>(
        this SpecificationSet<Core.Entitites.Track> _, string name, Expression<Func<Core.Entitites.Track, TProjection>> selector)
        => new TrackByName<TProjection>(Predicate(name), selector);
}

internal sealed class TrackByName : Specification<Core.Entitites.Track>
{
    public TrackByName(Expression<Func<Core.Entitites.Track, bool>> predicate)
    {
        Query.Where(predicate);
    }   
}

internal sealed class TrackByName<TProjection> : Specification<Core.Entitites.Track, TProjection>
{
    public TrackByName(Expression<Func<Core.Entitites.Track, bool>> predicate, Expression<Func<Core.Entitites.Track, TProjection>> selector)
    {
        Query.Where(predicate);
        Query.Select(selector);
    }
}