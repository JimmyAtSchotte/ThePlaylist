using NHibernate.Criterion;
using ThePlaylist.Specifications;

namespace ThePlaylist.Specifications.Track;

public class TrackByNameCriteria : CriteriaSpecification<Core.Entitites.Track>
{
    public TrackByNameCriteria(string trackName)
    {
        this.UseCriteria(criteria => criteria.Add(Restrictions.Eq(nameof(Core.Entitites.Track.Name), trackName)));
    }
}