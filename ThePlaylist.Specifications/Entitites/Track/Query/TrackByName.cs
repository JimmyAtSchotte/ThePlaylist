using Ardalis.Specification;

namespace ThePlaylist.Specifications.Entitites.Track.Query;


public static partial class SpecificationSetExtensions
{
    public static Specification<Core.Entitites.Track> TrackByName(
        this SpecificationSet<Core.Entitites.Track> set, string name)
        => new TrackByName(name);
}

internal sealed class TrackByName : Specification<Core.Entitites.Track>
{
    public TrackByName(string trackName)
    {
        Query.Where(x => x.Name == trackName);
    }
}