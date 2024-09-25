using Ardalis.Specification;
using FluentAssertions;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;
using ThePlaylist.Specifications;

namespace ThePlaylist.Infrastructure.Tests.Repository;

[TestFixture]
public class Relationships
{
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void ManyToManyRelationship(IRepositorySource repositoryProvider)
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
    public void SelfReferencingRelationship(IRepositorySource repositoryProvider)
    {
        var rock = new Genre() { Name = Guid.NewGuid().ToString() };
        var metal = new Genre() { Name = Guid.NewGuid().ToString() };
        rock.AddSubGenre(metal);
        
        using var repository = repositoryProvider.CreateRepository();
        repository.Add(rock);

        var fetchedGenre = repository.Get<Genre>(rock.Id);
        fetchedGenre.SubGenres.Should().HaveCount(1);
    }
}