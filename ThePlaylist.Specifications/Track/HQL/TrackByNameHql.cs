using Ardalis.Specification;

namespace ThePlaylist.Specifications.Track.HQL;

public class TrackByNameHql(string trackName) : HqlSpecification<Core.Entitites.Track>(session => session
    .CreateQuery("from Track t where t.Name = :Name")
    .SetParameter("Name", trackName));