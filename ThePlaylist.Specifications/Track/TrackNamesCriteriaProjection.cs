using NHibernate.Criterion;
using NHibernate.Transform;

namespace ThePlaylist.Specifications.Track;

public class TrackNamesCriteriaProjection : CriteriaSpecification<Core.Entitites.Track, TrackName>
{
    public TrackNamesCriteriaProjection()
    {
        this.UseCriteria(criteria => criteria
                .SetProjection(Projections.ProjectionList()
                    .Add(Projections.Property<Core.Entitites.Track>(x => x.Name), nameof(TrackName.Name)))
                .SetResultTransformer(Transformers.AliasToBean<TrackName>())
        );
    }
}