using Ardalis.Specification;
using FluentAssertions;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;
using ThePlaylist.Specifications;

namespace ThePlaylist.Infrastructure.Tests.Relationships;

[TestFixture]
public class Genres
{
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void SubGenres(IRepositorySource repositoryProvider)
    {
        var rock = new Genre() { Name = "rock" };
        var metal = new Genre() { Name = "metal" };
        rock.AddSubGenre(metal);
        
        using var repository = repositoryProvider.CreateRepository(rock);

        var genre = repository.Get(Specs.ById<Genre>(rock.Id, query => query.Include(x => x.SubGenres)));
        genre.SubGenres.Should().HaveCount(1);
    }
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void Parent(IRepositorySource repositoryProvider)
    {
        var rock = new Genre() { Name = "rock" };
        var metal = new Genre() { Name = "metal" };
        rock.AddSubGenre(metal);
        
        using var repository = repositoryProvider.CreateRepository(rock);

        var genre = repository.Get(Specs.ById<Genre>(metal.Id, query => query.Include(x => x.Parent)));
        genre.Parent.Id.Should().Be(rock.Id);
    }
}