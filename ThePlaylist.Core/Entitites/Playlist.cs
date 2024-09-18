using System.Collections;
using System.Diagnostics.Contracts;

namespace ThePlaylist.Core.Entitites;

public class Playlist : IEntity
{
    private IEnumerable<Track> _tracks = new List<Track>();
    
    public virtual Guid Id { get; set; }
    public virtual string Name { get;  set; }
    public virtual string? Description { get; set; }

    public virtual IEnumerable<Track> Tracks
    {
        get => _tracks; 
        init => _tracks = value;
    } 

    public virtual Track AddTrack(Track track)
    {
        var tracks = _tracks.ToList();
        tracks.Add(track);
        track.AddPlaylist(this);
        _tracks = tracks;
        
        return track;
    }
}