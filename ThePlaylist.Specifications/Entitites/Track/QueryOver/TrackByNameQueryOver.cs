namespace ThePlaylist.Specifications.Entitites.Track.QueryOver;


public static partial class SpecificationSetExtensions
{
    public static QueryOverSpecification<Core.Entitites.Track> TrackByNameQueryOver(
        this SpecificationSet<Core.Entitites.Track> set, string trackName)
        => new TrackByNameQueryOver(trackName);
}
internal class TrackByNameQueryOver(string trackName)
    : QueryOverSpecification<Core.Entitites.Track>(queryOver => queryOver.Where(x => x.Name == trackName));