using Ardalis.Specification;
using ThePlaylist.Core.Projections;

namespace ThePlaylist.Specifications.Entitites.Playlist.Query;


public static partial class SpecificationSetExtensions
{
    public static ISpecification<Core.Entitites.Playlist> AllPlaylistNames(
        this SpecificationSet<Core.Entitites.Playlist> set)
        => new AllPlaylistNames();
}

internal sealed class AllPlaylistNames : Specification<Core.Entitites.Playlist, PlaylistName>
{
    public AllPlaylistNames()
    {
        Query.Select(x => new PlaylistName()
        {
            Name = x.Name,
        });
    }
}