using NHibernate.Transform;
using ThePlaylist.Specifications.Track.Projections;

namespace ThePlaylist.Specifications.Track.Criteria;

public class TrackNamesCriteriaProjection : CriteriaSpecification<Core.Entitites.Track, TrackName>
{
    public TrackNamesCriteriaProjection()
    {
        this.UseCriteria(criteria => criteria
                .SetProjection(NHibernate.Criterion.Projections.ProjectionList()
                    .Add(NHibernate.Criterion.Projections.Property<Core.Entitites.Track>(x => x.Name), nameof(TrackName.Name)))
                .SetResultTransformer(Transformers.AliasToBean<TrackName>())
        );
    }
}