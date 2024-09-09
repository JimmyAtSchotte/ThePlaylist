using System.Runtime.CompilerServices;
using Ardalis.Specification;
using ThePlaylist.Core.Entitites;

namespace ThePlaylist.Core.Specification;

public static class Specifications
{
    public static SpecificationBuilder<Genre> Genre => new (new GenericSpecification<Genre>());
    public static SpecificationBuilder<Track> Track => new (new GenericSpecification<Track>());
}