using System.Collections;
using System.Diagnostics.Contracts;

namespace ThePlaylist.Core.Entitites;

public class Playlist : PlaylistBase
{
    private IEnumerable<Track> _tracks = new List<Track>();
    private IEnumerable<PlaylistRevision> _revisions = new List<PlaylistRevision>();
    
    public virtual IEnumerable<Track> Tracks
    {
        get => _tracks; 
        init => _tracks = value;
    }

    public virtual IEnumerable<PlaylistRevision> Revisions
    {
        get => _revisions;
        set => _revisions = value;
    }

    public virtual Track AddTrack(Track track)
    {
        var tracks = _tracks.ToList();
        tracks.Add(track);
        track.AddPlaylist(this);
        _tracks = tracks;
        
        return track;
    }

    public virtual void RemoveTrack(Track track)
    {
        var tracks = _tracks.ToList();
        tracks.Remove(track);
        _tracks = tracks;
    }

    public virtual void CreateRevision(Action<Playlist> action)
    {
        var revisions = _revisions.ToList();
        revisions.Add(new PlaylistRevision(this));
        _revisions = revisions;
        Revision += 1;
        action(this);
    }
}