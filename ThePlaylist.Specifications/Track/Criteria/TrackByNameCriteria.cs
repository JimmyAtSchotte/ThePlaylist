using NHibernate.Criterion;

namespace ThePlaylist.Specifications.Track.Criteria;

public class TrackByNameCriteria : CriteriaSpecification<Core.Entitites.Track>
{
    public TrackByNameCriteria(string trackName)
    {
        this.UseCriteria(criteria => criteria.Add(Restrictions.Eq(nameof(Core.Entitites.Track.Name), trackName)));
    }
}