using Ardalis.Specification;

namespace ThePlaylist.Specifications.Entitites.Genre.Query;

public static partial class SpecificationSetExtensions
{
    public static ISpecification<Core.Entitites.Genre> ByName(this SpecificationSet<Core.Entitites.Genre> set,
        string name)
        => new GenreByNameQuery(name);
}

internal sealed class GenreByNameQuery : Specification<Core.Entitites.Genre>
{
    public GenreByNameQuery(string name)
    {
        Query.Where(g => g.Name == name);
    }
}