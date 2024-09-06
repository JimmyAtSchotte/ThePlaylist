namespace ThePlaylist.Core.Entitites;

public class Playlist
{
    private IList<Track> _tracks = new List<Track>();
    
    public virtual Guid Id { get; set; }
    public virtual string Name { get;  set; }
    public virtual string? Description { get; set; }

    public virtual IEnumerable<Track> Tracks
    {
        get => _tracks; 
        set => _tracks = value.ToList();
    } 

    public virtual void AddTrack(Track track)
    {
        _tracks.Add(track);
    }
}