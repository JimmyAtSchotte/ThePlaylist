using Ardalis.Specification;
using ThePlaylist.Core.Projections;

namespace ThePlaylist.Specifications.Playlist.Query;

public sealed class AllPlaylistNames : Specification<Core.Entitites.Playlist, PlaylistName>
{
    public AllPlaylistNames()
    {
        Query.Select(x => new PlaylistName()
        {
            Name = x.Name,
        });
    }
    
}