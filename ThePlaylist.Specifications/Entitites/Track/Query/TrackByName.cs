using Ardalis.Specification;

namespace ThePlaylist.Specifications.Entitites.Track.Query;

public sealed class TrackByName : Specification<Core.Entitites.Track>
{
    public TrackByName(string trackName)
    {
        Query.Where(x => x.Name == trackName);
    }
}