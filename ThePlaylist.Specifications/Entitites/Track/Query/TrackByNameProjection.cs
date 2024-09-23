using Ardalis.Specification;
using ThePlaylist.Core.Projections;

namespace ThePlaylist.Specifications.Entitites.Track.Query;


public static partial class SpecificationSetExtensions
{
    public static Specification<Core.Entitites.Track, TrackName> TrackByNameProjection(
        this SpecificationSet<Core.Entitites.Track> set, string name)
        => new TrackByNameProjection(name);
}

internal sealed class TrackByNameProjection : Specification<Core.Entitites.Track, TrackName>
{
    public TrackByNameProjection(string trackName)
    {
        Query.Where(x => x.Name == trackName);
        Query.Select(x => new TrackName()
        {
            Name = x.Name,
        });
    }
}