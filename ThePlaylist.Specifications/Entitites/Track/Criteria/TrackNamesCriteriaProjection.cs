using NHibernate.Transform;
using ThePlaylist.Core.Projections;

namespace ThePlaylist.Specifications.Entitites.Track.Criteria;

public static partial class SpecificationSetExtensions
{
    public static CriteriaSpecification<Core.Entitites.Track, TrackName> TrackNamesCriteriaProjection(this SpecificationSet<Core.Entitites.Track> set)
        => new TrackNamesCriteriaProjection();
}

internal class TrackNamesCriteriaProjection() : CriteriaSpecification<Core.Entitites.Track, TrackName>(criteria =>
    criteria.SetProjection(NHibernate.Criterion.Projections.ProjectionList()
            .Add(NHibernate.Criterion.Projections.Property<Core.Entitites.Track>(x => x.Name), nameof(TrackName.Name)))
            .SetResultTransformer(Transformers.AliasToBean<TrackName>()));