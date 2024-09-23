using NHibernate.Criterion;
using NHibernate.Transform;
using ThePlaylist.Core.Projections;

namespace ThePlaylist.Specifications.Entitites.Track.QueryOver;

public static partial class SpecificationSetExtensions
{
    public static QueryOverSpecification<Core.Entitites.Track, TrackName> TrackNamesQueryOverProjection(
        this SpecificationSet<Core.Entitites.Track> set)
        => new TrackNamesQueryOverProjection();
}

internal class TrackNamesQueryOverProjection() : QueryOverSpecification<Core.Entitites.Track, TrackName>(queryOver =>
    queryOver
        .SelectList(list => list
            .Select(NHibernate.Criterion.Projections.Property<Core.Entitites.Track>(x => x.Name)
                .WithAlias(nameof(TrackName.Name))))
        .TransformUsing(Transformers.AliasToBean<TrackName>()));