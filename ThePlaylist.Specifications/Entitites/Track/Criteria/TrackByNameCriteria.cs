using NHibernate.Criterion;

namespace ThePlaylist.Specifications.Entitites.Track.Criteria;

public class TrackByNameCriteria(string trackName) : CriteriaSpecification<Core.Entitites.Track>(criteria =>
    criteria.Add(Restrictions.Eq(nameof(Core.Entitites.Track.Name), trackName)));