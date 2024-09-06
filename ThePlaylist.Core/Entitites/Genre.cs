namespace ThePlaylist.Core.Entitites;

public class Genre
{
    private IList<Genre> _subGenres = new List<Genre>();
    private IEnumerable<Track> _tracks = new List<Track>();
    public virtual Guid Id { get; set; }
    public virtual string Name { get;  set; }
    public virtual Genre Parent { get; set; }

    public virtual IEnumerable<Genre> SubGenres
    {
        get => _subGenres.AsReadOnly();
        set => _subGenres = value.ToList();
    }

    public virtual IEnumerable<Track> Tracks
    {
        get => _tracks;
        set => _tracks = value;
    }

    public virtual void AddSubGenre(Genre genre)
    {
        _subGenres.Add(genre);
        genre.Parent = this;
    }
}