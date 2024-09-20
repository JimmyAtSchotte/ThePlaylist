using Ardalis.Specification;
using ThePlaylist.Core.Projections;

namespace ThePlaylist.Specifications.Playlist.Query;

public sealed class ByName : Specification<Core.Entitites.Playlist, PlaylistName>
{
    public ByName(string name)
    {
        Query.Where(x => x.Name == name);
    }
    
}