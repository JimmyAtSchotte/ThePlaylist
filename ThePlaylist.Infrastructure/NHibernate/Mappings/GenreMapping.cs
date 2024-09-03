using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using ThePlaylist.Core.Entitites;

namespace ThePlaylist.Infrastructure.NHibernate.Mappings;

public class GenreMapping : ClassMapping<Genre>
{
    public GenreMapping()
    {
        Table("Genres");
        Id(x => x.Id, map => map.Generator(Generators.NativeGuid));
        Property(x => x.Name, map =>
        {
            map.NotNullable(true);
            map.Length(255);
        });
    }
}