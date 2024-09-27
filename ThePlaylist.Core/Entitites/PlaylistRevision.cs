﻿namespace ThePlaylist.Core.Entitites;

public class PlaylistRevision : PlaylistBase
{
    public Guid PlaylistId { get; set; }
    public Playlist Playlist { get; set; }
    
    public PlaylistRevision()
    {
        
    }
    
    public PlaylistRevision(Playlist playlist)
    {
        this.Playlist = playlist;
        this.PlaylistId = Playlist.Id;
        this.Revision = playlist.Revision;
        this.Description = playlist.Description;
        this.Name = playlist.Name;
    }

}