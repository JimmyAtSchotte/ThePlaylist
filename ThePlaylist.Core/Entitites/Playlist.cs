namespace ThePlaylist.Core.Entitites;

public class Playlist
{
    public virtual Guid Id { get; private set; }
    public virtual string Name { get; private set; }
    public virtual string Description { get; private set; }
    public virtual IList<Track> Tracks { get; private set; } = new List<Track>();
}