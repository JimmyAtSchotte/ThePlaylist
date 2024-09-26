using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Hql.Ast;
using NHibernate.Tool.hbm2ddl;
using ThePlaylist.Core.Interfaces;
using ThePlaylist.Infrastructure.NHibernate;

namespace ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource.NHibernate;

public class NHibernateSqlExpressRepositorySource : IRepositorySource
{
    private IServiceProvider? _services;
    private IServiceProvider Services => _services ??= Initialize();

    private static IServiceProvider Initialize()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddNHibernate(options => options.UseSqlExpress("Server=1337-JIMMY\\SQLEXPRESS;Database=ThePlaylist_Test_NH;Trusted_Connection=True;"));
        serviceCollection.AddTransient<IRepository, Infrastructure.NHibernate.Repository>();
        
        var services = serviceCollection.BuildServiceProvider();
        var configuration = services.GetRequiredService<Configuration>();
        var session = services.GetRequiredService<ISession>();
        new SchemaExport(configuration).Execute(true, true, false, session.Connection, null);

        using var command = session.Connection.CreateCommand();
        command.CommandText = @"
                ALTER TABLE PlaylistTracks DROP CONSTRAINT FK_PlaylistTracks_Track;
                ALTER TABLE PlaylistTracks DROP CONSTRAINT FK_PlaylistTracks_Playlist;

                ALTER TABLE PlaylistTracks
                ADD CONSTRAINT FK_PlaylistTracks_Track
                FOREIGN KEY (TrackId)
                REFERENCES Tracks (Id)
                ON DELETE CASCADE;
                
                ALTER TABLE PlaylistTracks
                ADD CONSTRAINT FK_PlaylistTracks_Playlist
                FOREIGN KEY (PlaylistId)
                REFERENCES Playlists (Id)
                ON DELETE CASCADE;
            ";
        command.ExecuteNonQuery();

        return services;
    }

    public IRepository CreateRepository()
    {
        return Services.GetRequiredService<IRepository>();
    }

    public IRepository CreateRepository(params object[] entities)
    {
        var session = Services.GetRequiredService<ISession>();

        foreach (var variable in entities)
            session.Save(variable);
        
        session.Flush();
        session.Clear();
        
        return Services.GetRequiredService<IRepository>();
    }
}