using Ardalis.Specification;

namespace ThePlaylist.Specifications.Entitites.Playlist.Query;


public static partial class SpecificationSetExtensions
{
    public static ISpecification<Core.Entitites.Playlist> AllTracksAllGenres(
        this SpecificationSet<Core.Entitites.Playlist> set)
        => new AllPlaylistNames();
}

internal sealed class AllTracksAllGenres : Specification<Core.Entitites.Playlist>
{
    public AllTracksAllGenres()
    {
        Query.Include(x => x.Tracks).ThenInclude(x => x.Genres);
    }
    
}