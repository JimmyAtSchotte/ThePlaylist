using Ardalis.Specification;

namespace ThePlaylist.Infrastructure.Tests.NHibernate.Specifications.Track;

public class TrackByName : Specification<Core.Entitites.Track>
{
    public TrackByName(string trackName)
    {
        Query.Where(x => x.Name == trackName);
    }
}