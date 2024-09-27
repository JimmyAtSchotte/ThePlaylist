namespace ThePlaylist.Core.Entitites;

public abstract class PlaylistBase : IEntity
{
    public virtual Guid Id { get; set; }
    public virtual int Revision { get; set; }
    public virtual string Name { get;  set; }
    public virtual string? Description { get; set; }
}