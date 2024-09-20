using Ardalis.Specification;
using FluentAssertions;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Infrastructure.Exceptions;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;
using ThePlaylist.Infrastructure.Tests.Specifications.Track.Query;
using ThePlaylist.Specifications;
using ThePlaylist.Specifications.Genre.Query;
using ThePlaylist.Specifications.Track.HQL;
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
    public async Task AddEntityAsync(RepositorySource repositoryProvider)
    {
        var playlist = new Playlist()
        {
            Name = "My playlist",
            Description = "My playlist description"
        };

        await using var repository = repositoryProvider.CreateRepository();

        var savesPlaylist = await repository.AddAsync(playlist);
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
    public async Task ListEntitiesAsync(RepositorySource repositoryProvider)
    {
        var playlist = new Playlist()
        {
            Name = "My playlist 2",
            Description = "My playlist 2 description"
        };
        await using var repository = repositoryProvider.CreateRepository();
        
        await repository.AddAsync(playlist);
        
        var playlists = await repository.ListAsync<Playlist>();
        playlists.Should().Contain(x => x.Id == playlist.Id);
    }
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void ListEntitiesBySpecification(RepositorySource repositoryProvider)
    {
        var playlist = new Playlist()
        {
            Name = Guid.NewGuid().ToString(),
            Description = "My playlist 2 description"
        };
        using var repository = repositoryProvider.CreateRepository();
        
        repository.Add(playlist);
        repository.List<Playlist>(new ThePlaylist.Specifications.Playlist.Query.ByName(playlist.Name)).Should().Contain(x => x.Id == playlist.Id);
    }
    
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public async Task ListEntitiesBySpecificationAsync(RepositorySource repositoryProvider)
    {
        var playlist = new Playlist()
        {
            Name = Guid.NewGuid().ToString(),
            Description = "My playlist 2 description"
        };
        await using var repository = repositoryProvider.CreateRepository();
        
        await repository.AddAsync(playlist);
        (await repository.ListAsync<Playlist>(new ThePlaylist.Specifications.Playlist.Query.ByName(playlist.Name))).Should().Contain(x => x.Id == playlist.Id);
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
    public async Task DeleteEntityAsync(RepositorySource repositoryProvider)
    {
        var playlist = new Playlist()
        {
            Name = "My playlist 3",
            Description = "My playlist 3 description"
        };
        await using var repository = repositoryProvider.CreateRepository();
        
        var savesPlaylist = await repository.AddAsync(playlist);
        await repository.DeleteAsync(savesPlaylist);
        
        var playlists = await repository.ListAsync<Playlist>();
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
    public async Task UpdateEntityAsync(RepositorySource repositoryProvider)
    {
        var playlist = new Playlist()
        {
            Name = "My playlist 4",
            Description = "My playlist 4 description"
        };

        await using var repository = repositoryProvider.CreateRepository();
        var savesPlaylist = await repository.AddAsync(playlist);
        savesPlaylist.Description = "UPDATED";
        
       await repository.UpdateAsync(savesPlaylist);
        
        var playlists = await repository.ListAsync<Playlist>();
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
        
        repository.Get<Playlist>(playlist.Id).Should().NotBeNull();
        repository.Get<Playlist>(new ById<Playlist>(playlist.Id)).Should().NotBeNull();
       
    }
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public async Task GetEntityAsync(RepositorySource repositoryProvider)
    {
        var playlist = new Playlist()
        {
            Name = "My playlist 5",
            Description = "My playlist 5 description"
        };

        await using var repository = repositoryProvider.CreateRepository();
        await repository.AddAsync(playlist);
        
       (await repository.GetAsync<Playlist>(playlist.Id)).Should().NotBeNull();
       (await repository.GetAsync<Playlist>(new ById<Playlist>(playlist.Id))).Should().NotBeNull();
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
    public void RollbackOnAggregateRootError(RepositorySource repositoryProvider)
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
    
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void RollbackUnitOfWork(RepositorySource repositoryProvider)
    {
        var genreA = new Genre() { Name = Guid.NewGuid().ToString() };
        var genreB = new Genre() { Name = genreA.Name };
        var genres = new List<Genre>() { genreA, genreB };
        
        using var repository = repositoryProvider.CreateRepository();

        try
        {
            repository.ExecuteUnitOfWork(r =>
            {
                foreach (var genre in genres)
                    r.Add(genre);
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        repository.Invoking(r => r.Get(new GenreByNameQuery(genreA.Name))).Should().Throw<EntityNotFoundException>();
    }
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void NestedUnitOfWorks(RepositorySource repositoryProvider)
    {
        var genre = new Genre() { Name = Guid.NewGuid().ToString() };
        var genreA = new Genre() { Name = Guid.NewGuid().ToString() };
        var genreB = new Genre() { Name = genreA.Name };
        var genres = new List<Genre>() { genreA, genreB };
        
        using var repository = repositoryProvider.CreateRepository();

        try
        {
            repository.ExecuteUnitOfWork(r =>
            {
                r.Add(genre);
            
                r.ExecuteUnitOfWork(r2 =>
                {
                    foreach (var g in genres)
                        r2.Add(g);
                });
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
      

        repository.Invoking(r => r.Get(new GenreByNameQuery(genre.Name))).Should().Throw<EntityNotFoundException>();
        repository.Invoking(r => r.Get(new GenreByNameQuery(genreA.Name))).Should().Throw<EntityNotFoundException>();
    }
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void MultipleUnitOfWorks(RepositorySource repositoryProvider)
    {
        var genre = new Genre() { Name = Guid.NewGuid().ToString() };
        var genreA = new Genre() { Name = Guid.NewGuid().ToString() };
        var genreB = new Genre() { Name = genreA.Name };
        var genres = new List<Genre>() { genreA, genreB };
        
        using var repository = repositoryProvider.CreateRepository();

        repository.ExecuteUnitOfWork(r =>
        {
            r.Add(genre);
        });

        try
        {
            repository.ExecuteUnitOfWork(r =>
            {
                foreach (var g in genres)
                    r.Add(g);
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
      

        repository.Invoking(r => r.Get(new GenreByNameQuery(genre.Name))).Should().NotThrow();
        repository.Invoking(r => r.Get(new GenreByNameQuery(genreA.Name))).Should().Throw<EntityNotFoundException>();
    }
    
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void Projection(RepositorySource repositoryProvider)
    {
        var track = new Core.Entitites.Track
        {
            Name = Guid.NewGuid().ToString()
        };
        
        using var repository = repositoryProvider.CreateRepository();
        repository.Add(track);
        
        repository.List(new TrackByNameProjection(track.Name)).Should().Contain(x => x.Name == track.Name);
    }
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public async Task ProjectionAsync(RepositorySource repositoryProvider)
    {
        var track = new Core.Entitites.Track
        {
            Name = Guid.NewGuid().ToString()
        };
        
        await using var repository = repositoryProvider.CreateRepository();
        await repository.AddAsync(track);
        (await repository.ListAsync(new TrackByNameProjection(track.Name))).Should().Contain(x => x.Name == track.Name);
    }
}