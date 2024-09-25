using FluentAssertions;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;

namespace ThePlaylist.Infrastructure.Tests.Repository;

[TestFixture]
public class AddEntity
{
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void Add(IRepositorySource repositoryProvider)
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
    public async Task AddAsync(IRepositorySource repositoryProvider)
    {
        var playlist = new Playlist()
        {
            Name = "My playlist",
            Description = "My playlist description"
        };

        await using var repository = repositoryProvider.CreateRepository();

        var savesPlaylist = await repository.AddAsync(playlist, CancellationToken.None);
        savesPlaylist.Id.Should().NotBe(Guid.Empty);
    }
}