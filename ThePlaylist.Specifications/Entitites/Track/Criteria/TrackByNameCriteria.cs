using Ardalis.Specification;
using NHibernate.Criterion;

namespace ThePlaylist.Specifications.Entitites.Track.Criteria;

public static partial class SpecificationSetExtensions
{
    public static ISpecification<Core.Entitites.Track> TrackByNameCriteria(this SpecificationSet<Core.Entitites.Track> set,
        string name)
        => new TrackByNameCriteria(name);
}

internal class TrackByNameCriteria(string trackName) : CriteriaSpecification<Core.Entitites.Track>(criteria =>
    criteria.Add(Restrictions.Eq(nameof(Core.Entitites.Track.Name), trackName)));