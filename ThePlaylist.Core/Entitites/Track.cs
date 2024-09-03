namespace ThePlaylist.Core.Entitites;

public class Track
{
    public virtual Guid Id { get;  set; }
    public virtual string Name { get;  set; }
    public virtual IList<Genre> Genres { get; set; } = new List<Genre>();
    public virtual IEnumerable<Playlist> Playlists { get; set; } = new List<Playlist>();
}