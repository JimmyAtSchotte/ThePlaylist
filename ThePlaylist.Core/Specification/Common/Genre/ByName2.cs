using ThePlaylist.Core.Specification.Criterion;

namespace ThePlaylist.Core.Specification.Common.Genre;


public static partial class GenreSpecifications
{
    public static SpecificationBuilder<Entitites.Genre>  ByName2(this SpecificationBuilder<Entitites.Genre> builder, string name)
    {
        builder.Where(x => x.Name == name);

        return builder;
    }
}

