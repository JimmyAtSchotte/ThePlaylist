using ThePlaylist.Core.Specification.Criterion;

namespace ThePlaylist.Core.Specification.Common.Genre;


public static partial class GenreSpecifications
{
    public static SpecificationBuilder<Entitites.Genre>  ByName(this SpecificationBuilder<Entitites.Genre> builder, string name)
    {
        builder.Where(Restrictions.Eq(nameof(Entitites.Genre.Name), name));

        return builder;
    }
}

