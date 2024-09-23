using Ardalis.Specification;

namespace ThePlaylist.Specifications.Entitites.Genre.Query;

public sealed class GenreByNameQuery : Specification<Core.Entitites.Genre>
{
    public GenreByNameQuery(string name)
    {
        Query.Where(g => g.Name == name);
    }
}