using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using ThePlaylist.Core.Entitites;

namespace ThePlaylist.Infrastructure.NHibernate.Mappings;

public class PlaylistMapping : ClassMapping<Playlist>
{
    public PlaylistMapping()
    {
        Table("Playlists");
        Id(x => x.Id, map => map.Generator(Generators.NativeGuid));
        Property(x => x.Name, map =>
        {
            map.NotNullable(true);
            map.Length(255);
        });
        Property(x => x.Description, map =>
        {
            map.Length(1024);
        });
        Bag(x => x.Tracks, map =>
        {
            map.Table("PlaylistTracks");
            map.Key(x => x.Column("PlaylistId"));
            map.Inverse(false);
            map.Cascade(Cascade.All);
        }, rel => rel.ManyToMany(x => x.Column("TrackId")));
    }
}