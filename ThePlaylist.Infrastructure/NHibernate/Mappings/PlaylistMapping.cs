using NHibernate;
using NHibernate.Mapping;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;
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
        
        Property(x => x.Revision, map => map.NotNullable(true));
        Property(x => x.Description, map =>
        {
            map.Length(1024);
        });
        Set(x => x.Tracks, map =>
        {
            map.Table("PlaylistTracks");
            map.Key(x =>
            {
                x.Column("PlaylistId");
                x.ForeignKey("FK_PlaylistTracks_Playlist");
            });
            map.Cascade(Cascade.Persist | Cascade.Merge);
        }, rel => rel.ManyToMany(x => x.Column("TrackId")));
        
        Set(x => x.Revisions, map =>
        {
            map.Key(x =>
            {
                x.Column("PlaylistId");
            });
            map.Inverse(true);
            map.Cascade(Cascade.Persist | Cascade.Merge);
        }, rel => rel.OneToMany());
        
        
        Discriminator(x =>
        {
            x.Column("IsCurrent");
            x.Type<BooleanType>();
        });
        DiscriminatorValue(true);
    }
}

public class PlaylistRevisionMapping : ClassMapping<PlaylistRevision>
{
    public PlaylistRevisionMapping()
    {
        Table("Playlists");
        Id(x => x.Id, map =>
        {
            map.Generator(Generators.NativeGuid);
            map.Type<GuidType>();
        });
        
        Property(x => x.Revision, map => map.NotNullable(true));
        Property(x => x.Name, map =>
        {
            map.NotNullable(true);
            map.Length(255);
        });
        Property(x => x.Description, map =>
        {
            map.Length(1024);
        });
        
        ManyToOne(x => x.Playlist, map =>
        {
            map.Column("PlaylistId");
            map.ForeignKey("FK_PlaylistRevisions_Playlist");
            map.Cascade(Cascade.None);
        });
        
        Discriminator(x =>
        {
            x.Column("IsCurrent");
            x.Type<BooleanType>();
        });
        DiscriminatorValue(false);
    }
}

