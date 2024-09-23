using Ardalis.Specification;
using ThePlaylist.Core.Projections;

namespace ThePlaylist.Specifications.Entitites.Track.Query;

public sealed class TrackByNameProjection : Specification<Core.Entitites.Track, TrackName>
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