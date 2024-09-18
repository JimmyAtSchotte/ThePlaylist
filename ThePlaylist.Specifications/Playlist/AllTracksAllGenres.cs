﻿using Ardalis.Specification;

namespace ThePlaylist.Specifications.Playlist;

public sealed class AllTracksAllGenres : Specification<Core.Entitites.Playlist>
{
    public AllTracksAllGenres()
    {
        Query.Include(x => x.Tracks).ThenInclude(x => x.Genres);
    }
    
}