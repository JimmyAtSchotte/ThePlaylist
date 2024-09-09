namespace ThePlaylist.Core.Specification;

public static class Specifications
{
    public static SpecificationBuilder<Entitites.Genre> Genre => new SpecificationBuilder<Entitites.Genre>();
    public static SpecificationBuilder<Entitites.Track> Track => new SpecificationBuilder<Entitites.Track>();
}