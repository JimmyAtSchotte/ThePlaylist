using NHibernate.Transform;
using ThePlaylist.Core.Projections;

namespace ThePlaylist.Specifications.Entitites.Track.HQL;


public static partial class SpecificationSetExtensions
{
    public static HqlSpecification<Core.Entitites.Track, TrackName> TrackNamesHqlTransformerProjection(this SpecificationSet<Core.Entitites.Track> set)
        => new TrackNamesHqlTransformerProjection();
}

internal class TrackNamesHqlTransformerProjection() : HqlSpecification<Core.Entitites.Track, TrackName>(session => session
    .CreateQuery($"select t.Name as {nameof(TrackName.Name)} from Track t")
    .SetResultTransformer(Transformers.AliasToBean<TrackName>()));