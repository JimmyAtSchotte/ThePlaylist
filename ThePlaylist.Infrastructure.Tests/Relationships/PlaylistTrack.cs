using Ardalis.Specification;
using FluentAssertions;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;
using ThePlaylist.Specifications;

namespace ThePlaylist.Infrastructure.Tests.Relationships;

[TestFixture]
public class PlaylistTrack
{

    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void AddNewTrackToPlaylist(IRepositorySource repositorySource)
    {
        var playlist = new Playlist()
        {
            Name = "TestPlaylist",
        };

        var track = playlist.AddTrack(new Track()
        {
            Name = "TestTrack",
        });

        using var repository = repositorySource.CreateRepository();
        repository.Add(playlist);

        track.Id.Should().NotBe(Guid.Empty);
        
        var reloadedPlaylist = repository.Get(Specs.ById<Playlist>(playlist.Id, query => query.Include(x => x.Tracks)));
        reloadedPlaylist.Tracks.Should().HaveCount(1);
    }
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void AddExistingTrackToPlaylist(IRepositorySource repositorySource)
    {
        var playlist = new Playlist()
        {
            Name = "TestPlaylist",
        };

        var track = new Track()
        {
            Name = "TestTrack",
        };

        using var repository = repositorySource.CreateRepository(track, playlist);
        var existingTrack = repository.Get<Track>(track.Id);
        var existingPlaylist = repository.Get<Playlist>(playlist.Id);

        existingPlaylist.AddTrack(existingTrack);
        repository.Update(existingPlaylist);
        
        var reloadedPlaylist = repository.Get(Specs.ById<Playlist>(playlist.Id, query => query.Include(x => x.Tracks)));
        reloadedPlaylist.Tracks.Should().HaveCount(1);
    }
    
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void AddMultipleTracksToPlaylist(IRepositorySource repositorySource)
    {
        var playlist = new Playlist()
        {
            Name = "TestPlaylist",
        };

        playlist.AddTrack(new Track()
        {
            Name = "TestTrack 1",
        });
        
        playlist.AddTrack(new Track()
        {
            Name = "TestTrack 2",
        });

        using var repository = repositorySource.CreateRepository(playlist);
        
        var reloadedPlaylist = repository.Get(Specs.ById<Playlist>(playlist.Id, query => query.Include(x => x.Tracks)));
        reloadedPlaylist.Tracks.Should().HaveCount(2);
    }
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void RemoveTrackFromPlaylist(IRepositorySource repositorySource)
    {
        var playlist = new Playlist()
        {
            Name = "TestPlaylist",
        };

        var track1 = playlist.AddTrack(new Track()
        {
            Name = "TestTrack 1",
        });
        
        var track2 = playlist.AddTrack(new Track()
        {
            Name = "TestTrack 2",
        });

        using var repository = repositorySource.CreateRepository(playlist);
        var existingTrack = repository.Get<Track>(track1.Id);
        var existingPlaylist = repository.Get(Specs.ById<Playlist>(playlist.Id, query => query.Include(x => x.Tracks)));
        existingPlaylist.RemoveTrack(existingTrack);
        
        repository.Update(existingPlaylist);
        
        var reloadedPlaylist = repository.Get(Specs.ById<Playlist>(playlist.Id, query => query.Include(x => x.Tracks)));
        reloadedPlaylist.Tracks.Should().HaveCount(1);
    }
    
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void CascadeDeleteTrack(IRepositorySource repositorySource)
    {
        var playlist = new Playlist()
        {
            Name = "TestPlaylist",
        };

        var track1 = playlist.AddTrack(new Track()
        {
            Name = "TestTrack 1",
        });
        
        var track2 = playlist.AddTrack(new Track()
        {
            Name = "TestTrack 2",
        });

        using var repository = repositorySource.CreateRepository(playlist);
        var existingTrack = repository.Get<Track>(track1.Id);
        repository.Delete(existingTrack);
        
        var reloadedPlaylist = repository.Get(Specs.ById<Playlist>(playlist.Id, query => query.Include(x => x.Tracks)));
        reloadedPlaylist.Tracks.Should().HaveCount(1);
    }
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void CascadeDeletePlaylist(IRepositorySource repositorySource)
    {
        var playlist = new Playlist()
        {
            Name = "TestPlaylist",
        };

        var track = playlist.AddTrack(new Track()
        {
            Name = "TestTrack 1",
        });
        
        using var repository = repositorySource.CreateRepository(playlist);
        var existingPlaylist = repository.Get<Playlist>(playlist.Id);
        repository.Delete(existingPlaylist);
        
        var reloadedTrack = repository.Get(Specs.ById<Track>(track.Id, query => query.Include(x => x.Playlists)));
        reloadedTrack.Playlists.Should().HaveCount(0);
    }
}