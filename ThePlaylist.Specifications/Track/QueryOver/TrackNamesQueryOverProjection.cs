using NHibernate.Criterion;
using NHibernate.Transform;
using ThePlaylist.Core.Projections;

namespace ThePlaylist.Specifications.Track.QueryOver;

public class TrackNamesQueryOverProjection() : QueryOverSpecification<Core.Entitites.Track, TrackName>(queryOver =>
    queryOver
        .SelectList(list => list
            .Select(NHibernate.Criterion.Projections.Property<Core.Entitites.Track>(x => x.Name)
                .WithAlias(nameof(TrackName.Name))))
        .TransformUsing(Transformers.AliasToBean<TrackName>()));