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
    criteria.Add(Restrictions.Eq(nameof(Core.Entitites.Track.Name), trackName))), IEquatable<TrackByNameCriteria>
{
    private readonly string _trackName = trackName;
    
    public bool Equals(TrackByNameCriteria? other)
    {
        return _trackName == other?._trackName;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((TrackByNameCriteria)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_trackName);
    }
}