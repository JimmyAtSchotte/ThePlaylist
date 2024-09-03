namespace ThePlaylist.Core.Entitites;

public class Track
{
    public virtual Guid Id { get; private set; }
    public virtual string Name { get; private set; }
    public virtual IList<Genre> Genres { get; private set; } = new List<Genre>();
}