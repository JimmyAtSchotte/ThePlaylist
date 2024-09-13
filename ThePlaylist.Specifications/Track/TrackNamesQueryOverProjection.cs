using NHibernate.Criterion;
using NHibernate.Transform;

namespace ThePlaylist.Specifications.Track;

public class TrackNamesQueryOverProjection : QueryOverSpecification<Core.Entitites.Track, TrackName>
{
    public TrackNamesQueryOverProjection()
    {
        this.UseQueryOver(queryOver => queryOver
            .SelectList(list => list
                .Select(Projections.Property<Core.Entitites.Track>(x => x.Name).WithAlias(nameof(TrackName.Name))))
            .TransformUsing(Transformers.AliasToBean<TrackName>()));
    }
}