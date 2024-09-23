namespace ThePlaylist.Specifications.Entitites.Track.HQL;


public static partial class SpecificationSetExtensions
{
    public static HqlSpecification<Core.Entitites.Track> TrackByNameHql(this SpecificationSet<Core.Entitites.Track> set,
        string trackName)
        => new TrackByNameHql(trackName);
}


internal class TrackByNameHql(string trackName) : HqlSpecification<Core.Entitites.Track>(session => session
    .CreateQuery("from Track t where t.Name = :Name")
    .SetParameter("Name", trackName));