namespace ThePlaylist.Core.Entitites;

public class Track
{
    private IList<Genre> _genres = new List<Genre>();
    private IEnumerable<Playlist> _playlists = new List<Playlist>();
    public virtual Guid Id { get;  set; }
    public virtual string Name { get;  set; }

    public virtual IList<Genre> Genres
    {
        get => _genres;
        set => _genres = value;
    }

    public virtual IEnumerable<Playlist> Playlists
    {
        get => _playlists;
        set => _playlists = value;
    }
}