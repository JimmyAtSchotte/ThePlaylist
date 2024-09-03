namespace ThePlaylist.Core.Entitites;

public class Playlist
{
    public virtual Guid Id { get; set; }
    public virtual string Name { get;  set; }
    public virtual string Description { get; set; }
    public virtual IList<Track> Tracks { get; set; } = new List<Track>();
}