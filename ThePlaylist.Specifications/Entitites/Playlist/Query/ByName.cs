using Ardalis.Specification;
using ThePlaylist.Core.Projections;

namespace ThePlaylist.Specifications.Entitites.Playlist.Query;


public static partial class SpecificationSetExtensions
{
    public static ISpecification<Core.Entitites.Playlist> ByName(this SpecificationSet<Core.Entitites.Playlist> set,
        string name)
        => new ByName(name);
}

internal sealed class ByName : Specification<Core.Entitites.Playlist, PlaylistName>
{
    public ByName(string name)
    {
        Query.Where(x => x.Name == name);
    }
}