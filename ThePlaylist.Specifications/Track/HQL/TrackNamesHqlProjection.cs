using NHibernate.Transform;
using ThePlaylist.Core.Projections;

namespace ThePlaylist.Specifications.Track.HQL;

public class TrackNamesHqlProjection : HqlSpecification<Core.Entitites.Track, TrackName>
{
    public TrackNamesHqlProjection()
    {
        this.UseHql(session => session
            .CreateQuery($"select new {nameof(TrackName)}(t.{nameof(TrackName.Name)}) from Track t"));
    }
}