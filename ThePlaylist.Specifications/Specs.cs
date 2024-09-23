using ThePlaylist.Core.Entitites;

namespace ThePlaylist.Specifications;

public static class Specs
{
    public static SpecificationSet<Playlist> Playlist => new SpecificationSet<Playlist>();
    public static SpecificationSet<Track> Track => new SpecificationSet<Track>();
    public static SpecificationSet<Genre> Genre => new SpecificationSet<Genre>();
}

public class SpecificationSet<T>
{
    
}

