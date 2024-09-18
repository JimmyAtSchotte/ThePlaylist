using Ardalis.Specification;
using FluentAssertions;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Infrastructure.Exceptions;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;
using ThePlaylist.Specifications;
using ThePlaylist.Specifications.Track.Query;

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
    public void EntityNotFound(RepositorySource repositoryProvider)
    {
        using var repository = repositoryProvider.CreateRepository();

        repository.Invoking(r => r.Get<Playlist>(Guid.NewGuid())).Should().Throw<EntityNotFoundException>();

    }
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void ManyToManyRelationship(RepositorySource repositoryProvider)
    {
        var rock = new Genre() { Name = Guid.NewGuid().ToString() };
        var pop = new Genre() { Name = Guid.NewGuid().ToString() };
        
        var trackA = new Track { Name = "Track A" };
        trackA.AddGenre(rock);
        
        var trackB = new Track { Name = "Track B" };
        trackB.AddGenre(pop);

        var playlistA = new Playlist { Name = "Playlist A" };
        var playlistB = new Playlist { Name = "Playlist B" };
        
        using var repository = repositoryProvider.CreateRepository();
        playlistA.AddTrack(trackA);
        playlistA.AddTrack(trackB);
        repository.Add(playlistA);

        playlistB.AddTrack(trackA);
        playlistB.AddTrack(trackB);
        repository.Add(playlistB);
        
        var fetchedPlaylistA = repository.Get(new ById<Playlist>(playlistA.Id, specification => specification.Include(x => x.Tracks)));
        var fetchedPlaylistB = repository.Get(new ById<Playlist>(playlistB.Id, specification => specification.Include(x => x.Tracks)));
        var fetchedTrackA = repository.Get(new ById<Track>(trackA.Id, specification => specification.Include(x => x.Playlists).Include(x => x.Genres)));
        var fetchedTrackB = repository.Get(new ById<Track>(trackB.Id, specification => specification.Include(x => x.Playlists).Include(x => x.Genres)));
        
        fetchedPlaylistA.Tracks.Should().HaveCount(2);
        fetchedPlaylistB.Tracks.Should().HaveCount(2);
        fetchedTrackA.Genres.Should().Contain(x => x.Id == rock.Id);
        fetchedTrackB.Genres.Should().Contain(x => x.Id == pop.Id);
        fetchedTrackA.Playlists.Should().HaveCount(2);
        fetchedTrackB.Playlists.Should().HaveCount(2);
       
    }
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void SelfReferencingRelationship(RepositorySource repositoryProvider)
    {
        var rock = new Genre() { Name = Guid.NewGuid().ToString() };
        var metal = new Genre() { Name = Guid.NewGuid().ToString() };
        rock.AddSubGenre(metal);
        
        using var repository = repositoryProvider.CreateRepository();
        repository.Add(rock);

        var fetchedGenre = repository.Get<Genre>(rock.Id);
        fetchedGenre.SubGenres.Should().HaveCount(1);
    }
    
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void RollbackOnError(RepositorySource repositoryProvider)
    {
        var trackA = new Track() { Name = Guid.NewGuid().ToString() };
        var genreA = trackA.AddGenre(new Genre() { Name = Guid.NewGuid().ToString() });
        
        var trackB = new Track() { Name = Guid.NewGuid().ToString() };
        trackB.AddGenre(new Genre() { Name = genreA.Name});
        
        using var repository = repositoryProvider.CreateRepository();
        repository.Add(trackA);

        repository.Invoking(r => r.Add(trackB)).Should().Throw<Exception>();
        repository.Invoking(r => r.Get(new TrackByName(trackB.Name))).Should().Throw<EntityNotFoundException>();
    }


}