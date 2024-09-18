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
            map.Unique(true);
        });
        
        Set(x => x.Tracks, map =>
        {
            map.Table("TrackGenres");
            map.Key(x => x.Column("GenreId"));
            map.Inverse(true);
            map.Cascade(Cascade.All);
        }, rel => rel.ManyToMany(x => x.Column("TrackId")));
        
        ManyToOne(x => x.Parent, map =>
        {
            map.Column("ParentGenreId");
            map.ForeignKey("FK_Genre_ParentGenre");
            map.Cascade(Cascade.None);
        });

        Set(x => x.SubGenres, map =>
        {
            map.Key(k => k.Column("ParentGenreId"));
            map.Inverse(true);
            map.Cascade(Cascade.All.Include(Cascade.DeleteOrphans));
        }, map => map.OneToMany());
        
        Lazy(false);
    }
}