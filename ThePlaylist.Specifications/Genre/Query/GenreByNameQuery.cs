using Ardalis.Specification;

namespace ThePlaylist.Specifications.Genre.Query;

public sealed class GenreByNameQuery : Specification<Core.Entitites.Genre>
{
    public GenreByNameQuery(string name)
    {
        Query.Where(g => g.Name == name);
    }
}