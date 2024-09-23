using Ardalis.Specification;
using ThePlaylist.Core.Projections;

namespace ThePlaylist.Specifications.Entitites.Playlist.Query;


public static partial class SpecificationSetExtensions
{
    public static ISpecification<Core.Entitites.Playlist> ByName(this SpecificationSet<Core.Entitites.Playlist> set,
        string name)
        => new ByName(name);
}

internal sealed class ByName : Specification<Core.Entitites.Playlist>, IEquatable<ByName>
{
    private readonly string _name;

    public ByName(string name)
    {
        _name = name;
        Query.Where(x => x.Name == name);
    }

    public bool Equals(ByName? other)
    {
        return _name == other?._name;
    }
    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ByName other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_name);
    }
}
