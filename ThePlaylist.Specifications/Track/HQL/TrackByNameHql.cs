using Ardalis.Specification;

namespace ThePlaylist.Specifications.Track.HQL;

public class TrackByNameHql : HqlSpecification<Core.Entitites.Track>
{
    public TrackByNameHql(string trackName)
    {
        this.UseHql(session => session
            .CreateQuery("from Track t where t.Name = :Name")
            .SetParameter("Name", trackName));
    }
}