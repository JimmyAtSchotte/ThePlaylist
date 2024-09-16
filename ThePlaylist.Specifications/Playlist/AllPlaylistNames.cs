using Ardalis.Specification;

namespace ThePlaylist.Specifications.Playlist;

public class AllPlaylistNames : Specification<Core.Entitites.Playlist, PlaylistName>
{
    public AllPlaylistNames()
    {
        Query.Select(x => new PlaylistName()
        {
            Name = x.Name,
        });
    }
    
}