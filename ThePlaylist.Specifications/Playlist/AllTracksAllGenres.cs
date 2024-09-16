using Ardalis.Specification;

namespace ThePlaylist.Specifications.Playlist;

public class AllTracksAllGenres : Specification<Core.Entitites.Playlist>
{
    public AllTracksAllGenres()
    {
        Query.Include(x => x.Tracks).ThenInclude(x => x.Genres);
    }
    
}


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

public class PlaylistName
{
    public string Name { get; set; }
}