using FluentAssertions;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;

namespace ThePlaylist.Infrastructure.Tests;

[TestFixture]
public class RepositoryTests
{
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void AddEntity(RepositorySource repositoryProvider)
    {
        var playlist = new Playlist()
        {
            Name = "My playlist",
            Description = "My playlist description"
        };
        
        using var repository = repositoryProvider.CreateRepository();

        var savesPlaylist = repository.Add(playlist);
        savesPlaylist.Id.Should().NotBe(Guid.Empty);
    }
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void ListEntities(RepositorySource repositoryProvider)
    {
        var playlist = new Playlist()
        {
            Name = "My playlist 2",
            Description = "My playlist 2 description"
        };
        using var repository = repositoryProvider.CreateRepository();
        
        repository.Add(playlist);
        
        var playlists = repository.List<Playlist>();
        playlists.Should().Contain(x => x.Id == playlist.Id);
    }
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void DeleteEntity(RepositorySource repositoryProvider)
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
    public void UpdateEntity(RepositorySource repositoryProvider)
    {
        var playlist = new Playlist()
        {
            Name = "My playlist 4",
            Description = "My playlist 4 description"
        };

        using var repository = repositoryProvider.CreateRepository();
        var savesPlaylist =repository.Add(playlist);
        savesPlaylist.Description = "UPDATED";
        
        repository.Update(savesPlaylist);
        
        var playlists = repository.List<Playlist>();
        playlists.First(x => x.Id == savesPlaylist.Id).Description.Should().Be("UPDATED");
    }
    
   
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void GetEntity(RepositorySource repositoryProvider)
    {
        var playlist = new Playlist()
        {
            Name = "My playlist 5",
            Description = "My playlist 5 description"
        };

        using var repository = repositoryProvider.CreateRepository();
        repository.Add(playlist);
        
        var result = repository.Get<Playlist>(playlist.Id);
        result.Should().NotBeNull();
    }
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void ManyToManyRelationship(RepositorySource repositoryProvider)
    {
        var rock = new Genre() { Name = "Rock" };
        var pop = new Genre() { Name = "Pop" };
        
        var trackA = new Track { Name = "Track A" };
        trackA.AddGenre(rock);
        
        var trackB = new Track { Name = "Track B" };
        trackB.AddGenre(pop);

        var playlistA = new Playlist { Name = "Playlist A" };
        var playlistB = new Playlist { Name = "Playlist B" };

        playlistA.AddTrack(trackA);
        playlistA.AddTrack(trackB);
    
        playlistB.AddTrack(trackA);
        playlistB.AddTrack(trackB);

        using var repository = repositoryProvider.CreateRepository();
        repository.Add(playlistA);
        repository.Add(playlistB);
        
        var fetchedPlaylistA = repository.Get<Playlist>(playlistA.Id);
        var fetchedPlaylistB = repository.Get<Playlist>(playlistB.Id);
    
        fetchedPlaylistA.Tracks.Should().HaveCount(2);
        fetchedPlaylistB.Tracks.Should().HaveCount(2);

        var fetchedTrackA = repository.Get<Track>(trackA.Id);
        var fetchedTrackB = repository.Get<Track>(trackB.Id);

        fetchedTrackA.Playlists.Should().HaveCount(2);
        fetchedTrackB.Playlists.Should().HaveCount(2);
        
        fetchedTrackA.Genres.Should().HaveCount(1);
        fetchedTrackB.Genres.Should().HaveCount(1);
    }
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void SelfReferencingRelationship(RepositorySource repositoryProvider)
    {
        var rock = new Genre() { Name = "Rock" };
        var metal = new Genre() { Name = "metal" };
        rock.AddSubGenre(metal);
        
        using var repository = repositoryProvider.CreateRepository();
        repository.Add(rock);

        var fetchedGenre = repository.Get<Genre>(rock.Id);
        fetchedGenre.SubGenres.Should().HaveCount(1);
    }


}