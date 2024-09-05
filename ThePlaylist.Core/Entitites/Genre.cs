namespace ThePlaylist.Core.Entitites;

public class Genre
{
    private IList<Genre> _subGenres = new List<Genre>();
    public virtual Guid Id { get; set; }
    public virtual string Name { get;  set; }
    public virtual Genre Parent { get; set; }

    public virtual IEnumerable<Genre> SubGenres
    {
        get => _subGenres.AsReadOnly();
        set => _subGenres = value.ToList();
    } 

    public virtual IEnumerable<Track> Tracks { get;  set; } = new List<Track>();

    public virtual void AddSubGenre(Genre genre)
    {
        _subGenres.Add(genre);
        genre.Parent = this;
    }
}