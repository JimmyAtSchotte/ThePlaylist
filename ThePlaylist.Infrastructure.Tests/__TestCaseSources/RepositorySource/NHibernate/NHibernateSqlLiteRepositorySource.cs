using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using ThePlaylist.Core.Interfaces;
using ThePlaylist.Infrastructure.NHibernate;

namespace ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource.NHibernate;

public class NHibernateSqlLiteRepositorySource : IRepositorySource
{
    private bool _initialized;

    private IRepository _repository;
    private ISession _session;

    private void Initialize()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddNHibernate(options => options.UseSqlLite());
        
        var services = serviceCollection.BuildServiceProvider();
        var configuration = services.GetRequiredService<Configuration>();
        
        _session = services.GetRequiredService<ISession>();
        _repository = new Infrastructure.NHibernate.Repository(_session);
        
        new SchemaExport(configuration).Execute(true, true, false, _session.Connection, null);
        
        using (var command = _session.Connection.CreateCommand())
        {
            command.CommandText = "PRAGMA foreign_keys = ON;";
            command.ExecuteNonQuery();
        }

        //Recreate tables that uses cascade delete constraints
        using (var command = _session.Connection.CreateCommand())
        {
            command.CommandText = @"
                    DROP TABLE PlaylistTracks;

                    CREATE TABLE PlaylistTracks (
                        PlaylistId BLOB NOT NULL,
                        TrackId BLOB NOT NULL,
                        PRIMARY KEY (PlaylistId, TrackId),
                        FOREIGN KEY (PlaylistId) REFERENCES Playlists(Id) ON DELETE CASCADE,
                        FOREIGN KEY (TrackId) REFERENCES Tracks(Id) ON DELETE CASCADE
                    );
                ";
            command.ExecuteNonQuery();
        }
        
        _initialized = true;
    }

    public IRepository CreateRepository()
    {
        if(!_initialized)
            Initialize();
        
        _session.Clear();
        
        return _repository;
    }
    
    public IRepository CreateRepository(params object[] entities)
    {
        if(!_initialized)
            Initialize();
        
        foreach (var variable in entities)
            _session.Save(variable);
        
        _session.Flush();
        _session.Clear();
        
        return _repository;
    }
}