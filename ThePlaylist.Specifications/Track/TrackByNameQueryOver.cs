using NHibernate.Criterion;
using ThePlaylist.Specifications;

namespace ThePlaylist.Specifications.Track;

public class TrackByNameQueryOver : QueryOverSpecification<Core.Entitites.Track>
{
    public TrackByNameQueryOver(string trackName)
    {
        this.UseQueryOver(queryOver => queryOver.Where(x => x.Name == trackName));
    }
}