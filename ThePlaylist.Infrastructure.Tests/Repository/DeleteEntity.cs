using FluentAssertions;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;
using ThePlaylist.Specifications;
using ThePlaylist.Specifications.Entitites.Playlist.Query;

namespace ThePlaylist.Infrastructure.Tests.Repository;

[TestFixture]
public class DeleteEntity
{
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void Delete(IRepositorySource repositoryProvider)
    {
        var playlist = new Playlist()
        {
            Name = "My playlist 3",
            Description = "My playlist 3 description"
        };
        using var repository = repositoryProvider.CreateRepository();
        
        var savesPlaylist =repository.Add(playlist);
        repository.Delete(savesPlaylist);
        
        var playlists = repository.List<Playlist>();
        playlists.Should().NotContain(x => x.Id == savesPlaylist.Id);
    }
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public async Task DeleteAsync(IRepositorySource repositoryProvider)
    {
        var playlist = new Playlist()
        {
            Name = "My playlist 3",
            Description = "My playlist 3 description"
        };
        await using var repository = repositoryProvider.CreateRepository();
        
        var savedPlaylist = await repository.AddAsync(playlist, CancellationToken.None);
        await repository.DeleteAsync(savedPlaylist, CancellationToken.None);
        
        var playlists = await repository.ListAsync<Playlist>(CancellationToken.None);
        playlists.Should().NotContain(x => x.Id == savedPlaylist.Id);
    }
    
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void DeleteChild(IRepositorySource repositoryProvider)
    {
        var playlist = new Playlist()
        {
            Name = "My playlist 4",
            Description = "My playlist 4 description"
        };

        var track1 = playlist.AddTrack(new Track()
        {
            Name = "My track 1",
        });
        
        var track2 = playlist.AddTrack(new Track()
        {
            Name = "My track 2",
        });

        using var repository = repositoryProvider.CreateRepository(playlist);
        var trackToDelete = repository.Get<Track>(track1.Id);
        repository.Delete(trackToDelete);
        
        var playlists = repository.List(Specs.Playlist.AllTracksAllGenres()).ToList();
        playlists[0].Tracks.Should().HaveCount(1);
    }
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void DeleteChildFromList(IRepositorySource repositoryProvider)
    {
        var playlist = new Playlist()
        {
            Name = "My playlist 4",
            Description = "My playlist 4 description"
        };

        var track1 = playlist.AddTrack(new Track()
        {
            Name = "My track 1",
        });
        
        var track2 = playlist.AddTrack(new Track()
        {
            Name = "My track 2",
        });

        using var repository = repositoryProvider.CreateRepository();
        
        repository.Add(playlist);
        playlist.DeleteTrack(track1);
        repository.Update(playlist);
        
        var playlists = repository.List(Specs.Playlist.AllTracksAllGenres()).ToList();
        
        playlists[0].Tracks.Should().HaveCount(1);
    }
}