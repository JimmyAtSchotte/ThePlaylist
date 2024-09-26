using Ardalis.Specification;
using FluentAssertions;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;
using ThePlaylist.Specifications;

namespace ThePlaylist.Infrastructure.Tests.Relationships;

[TestFixture]
public class TrackGenre
{
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void AddNewGenreToTrack(IRepositorySource repositorySource)
    {
        var track = new Track()
        {
            Name = "TestTrack",
        };

        var genre = track.AddGenre(new Genre()
        {
            Name = "TestGenre"
        });

        using var repository = repositorySource.CreateRepository();
        repository.Add(track);

        genre.Id.Should().NotBe(Guid.Empty);
        
        var reloadedTrack = repository.Get(Specs.ById<Track>(track.Id, query => query.Include(x => x.Genres)));
        reloadedTrack.Genres.Should().HaveCount(1);
    }
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void AddExistingGenreToTrack(IRepositorySource repositorySource)
    {
        var track = new Track()
        {
            Name = "TestTrack",
        };

        var genre = new Genre()
        {
            Name = "TestGenre"
        };

        using var repository = repositorySource.CreateRepository(track, genre);
        var existingTrack = repository.Get(Specs.ById<Track>(track.Id));
        var existingGenre = repository.Get(Specs.ById<Genre>(genre.Id));
        existingTrack.AddGenre(existingGenre);
        
        repository.Update(existingTrack);
        
        var reloadedTrack = repository.Get(Specs.ById<Track>(track.Id, query => query.Include(x => x.Genres)));
        reloadedTrack.Genres.Should().HaveCount(1);
    }
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void AddMultipleGenresToTrack(IRepositorySource repositorySource)
    {
        var track = new Track()
        {
            Name = "Test Track",
        };

        track.AddGenre(new Genre()
        {
            Name = "Test genre 1",
        });
        
        track.AddGenre(new Genre()
        {
            Name = "Test genre 2",
        });

        using var repository = repositorySource.CreateRepository(track);
        
        var reloadedTrack = repository.Get(Specs.ById<Track>(track.Id, query => query.Include(x => x.Genres)));
        reloadedTrack.Genres.Should().HaveCount(2);
    }

    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void RemoveGenreFromTrack(IRepositorySource repositorySource)
    {
        var track = new Track()
        {
            Name = "Test Track",
        };
        
        var genre1 = track.AddGenre(new Genre()
        {
            Name = "Test genre 1",
        });
        
        var genre2 = track.AddGenre(new Genre()
        {
            Name = "Test genre 2",
        });
        
        
        using var repository = repositorySource.CreateRepository(track);
        var existingTrack = repository.Get(Specs.ById<Track>(track.Id, query => query.Include(x => x.Genres)));
        var existingGenre = repository.Get(Specs.ById<Genre>(genre1.Id));

        existingTrack.RemoveGenre(existingGenre);

        repository.Update(existingTrack);
        
        var reloadedTrack = repository.Get(Specs.ById<Track>(track.Id, query => query.Include(x => x.Genres)));
        reloadedTrack.Genres.Should().HaveCount(1);

    }
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void CascadeDeleteGenre(IRepositorySource repositorySource)
    {
        var track = new Track()
        {
            Name = "Test Track",
        };
        
        var genre1 = track.AddGenre(new Genre()
        {
            Name = "Test genre 1",
        });
        
        var genre2 = track.AddGenre(new Genre()
        {
            Name = "Test genre 2",
        });
        
        
        using var repository = repositorySource.CreateRepository(track);
        var existingGenre = repository.Get(Specs.ById<Genre>(genre1.Id));
        repository.Delete(existingGenre);
        
        var reloadedTrack = repository.Get(Specs.ById<Track>(track.Id, query => query.Include(x => x.Genres)));
        reloadedTrack.Genres.Should().HaveCount(1);
    }
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void CascadeDeleteTrack(IRepositorySource repositorySource)
    {
        var track = new Track()
        {
            Name = "Test Track",
        };
        
        var genre = track.AddGenre(new Genre()
        {
            Name = "Test genre",
        });
        
        using var repository = repositorySource.CreateRepository(track);
        var existingTrack = repository.Get(Specs.ById<Track>(track.Id));
        repository.Delete(existingTrack);
        
        var reloadedGenre = repository.Get(Specs.ById<Genre>(genre.Id, query => query.Include(x => x.Tracks)));
        reloadedGenre.Tracks.Should().HaveCount(0);
    }
}