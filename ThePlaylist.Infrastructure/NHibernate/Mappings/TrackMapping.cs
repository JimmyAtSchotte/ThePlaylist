using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using ThePlaylist.Core.Entitites;

namespace ThePlaylist.Infrastructure.NHibernate.Mappings;

public class TrackMapping : ClassMapping<Track>
{
    public TrackMapping()
    {
        Table("Tracks");
        Id(x => x.Id, map => map.Generator(Generators.NativeGuid));
        Property(x => x.Name, map =>
        {
            map.NotNullable(true);
            map.Length(255);
        });
        
        Bag(x => x.Playlists, map =>
        {
            map.Table("PlaylistTracks");
            map.Key(x => x.Column("TrackId"));
            map.Inverse(true);
            map.Cascade(Cascade.All);
        }, rel => rel.ManyToMany(x => x.Column("PlaylistId")));
        
        
        Bag(x => x.Genres, map =>
        {
            map.Table("TrackGenres");
            map.Key(x => x.Column("TrackId"));
            map.Inverse(false);
            map.Cascade(Cascade.All);
        }, rel => rel.ManyToMany(x => x.Column("GenreId")));
    }
}



