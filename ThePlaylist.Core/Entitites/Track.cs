using System.Collections;

namespace ThePlaylist.Core.Entitites;

public class Track
{
    private IEnumerable<Genre> _genres = new List<Genre>();
    private IEnumerable<Playlist> _playlists = new List<Playlist>();
    public virtual Guid Id { get;  set; }
    public virtual string Name { get;  set; }

    public virtual IEnumerable<Genre> Genres
    {
        get => _genres;
        init => _genres = value;
    }

    public virtual IEnumerable<Playlist> Playlists
    {
        get => _playlists;
        init => _playlists = value;
    }

    public virtual Genre AddGenre(Genre genre)
    {
        var genres = _genres.ToList();
        genres.Add(genre);

        _genres = genres;
        
        return genre;
    }
}