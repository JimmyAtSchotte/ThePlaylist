namespace ThePlaylist.Core.Entitites;

public class Genre
{
    private IEnumerable<Genre> _subGenres = new List<Genre>();
    private IEnumerable<Track> _tracks = new List<Track>();
    public virtual Guid Id { get; set; }
    public virtual string Name { get;  set; }
    public virtual Genre Parent { get; set; }

    public virtual IEnumerable<Genre> SubGenres
    {
        get => _subGenres;
        init => _subGenres = value;
    }

    public virtual IEnumerable<Track> Tracks
    {
        get => _tracks;
        init => _tracks = value;
    }

    public virtual void AddSubGenre(Genre genre)
    {
        var subGenres = _subGenres.ToList();
        subGenres.Add(genre);
        _subGenres = subGenres;
        genre.Parent = this;
    }
}