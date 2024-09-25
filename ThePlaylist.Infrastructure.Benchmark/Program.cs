// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Infrastructure.EntityFramework;
using ThePlaylist.Infrastructure.NHibernate;
using ThePlaylist.Specifications;
using ThePlaylist.Specifications.Entitites.Playlist.Query;

BenchmarkRunner.Run<RepsoitoryBenchmark>();


public class RepsoitoryBenchmark
{
    private IServiceProvider services;
    
    private const string ConnnectionString = "Server=1337-JIMMY\\SQLEXPRESS;Database=ThePlaylist_Test;Trusted_Connection=True;TrustServerCertificate=True;";
    
    [GlobalSetup]
    public void Setup()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddNHibernate(options => options.UseSqlExpress(ConnnectionString, false, false));
        serviceCollection.AddTransient(services =>
            new ThePlaylist.Infrastructure.NHibernate.Repository(services.GetRequiredService<ISession>()));
        
        serviceCollection.AddDbContext<Context>(options => options.UseSqlServer(ConnnectionString));
      
        serviceCollection.AddTransient(services =>
            new ThePlaylist.Infrastructure.EntityFramework.Repository(services.GetRequiredService<Context>()));
   
        services = serviceCollection.BuildServiceProvider();
        
        SeedData();
    }

    private void SeedData()
    {
        var configuration = services.GetRequiredService<Configuration>();
        var session = services.GetRequiredService<ISession>();
        
        new SchemaExport(configuration).Execute(true, true, false, session.Connection, null);
        
        var repository = services.GetRequiredService<ThePlaylist.Infrastructure.NHibernate.Repository>();

        for (var playlistId = 0; playlistId < 100; playlistId++)
        {
            var playlist = new Playlist
            {
                Name = $"Playlist {playlistId}"
            };

            for (var trackId = 0; trackId < 10; trackId++)
            {
                var track = new Track
                {
                    Name = $"Track {playlistId}-{trackId}"
                };

                for (var genreId = 0; genreId < 2; genreId++)
                {
                    var genre = new Genre
                    {
                        Name = $"Genre  {playlistId}-{trackId}-{genreId}"
                    };

                    track.AddGenre(genre);
                }
                
                playlist.AddTrack(track);
            }

            repository.Add(playlist);
        }
    }


    [Benchmark]
    public void EFCore()
    {
        using var scope = services.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<ThePlaylist.Infrastructure.EntityFramework.Repository>();
        var playlists = repository.List<Playlist>(Specs.Playlist.AllTracksAllGenres());

        foreach (var playlist in playlists)
        {
            foreach (var track in playlist.Tracks)
            {
                foreach (var genre in track.Genres)
                {
                    
                }
            }
        }
    }
    
    [Benchmark]
    public void NHibernate()
    {
        using var scope = services.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<ThePlaylist.Infrastructure.NHibernate.Repository>();
        var playlists = repository.List<Playlist>(Specs.Playlist.AllTracksAllGenres());
        
        foreach (var playlist in playlists)
        {
            foreach (var track in playlist.Tracks)
            {
                foreach (var genre in track.Genres)
                {
                    
                }
            }
        }
    }
    
    
}