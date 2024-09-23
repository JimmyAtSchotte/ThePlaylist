using ThePlaylist.Core.Projections;

namespace ThePlaylist.Specifications.Entitites.Track.HQL;

public static partial class SpecificationSetExtensions
{
    public static HqlSpecification<Core.Entitites.Track, TrackName> TrackNamesHqlProjection(this SpecificationSet<Core.Entitites.Track> set)
        => new TrackNamesHqlProjection();
}

internal class TrackNamesHqlProjection() : HqlSpecification<Core.Entitites.Track, TrackName>(session => session
    .CreateQuery($"select new {nameof(TrackName)}(t.{nameof(TrackName.Name)}) from Track t"));