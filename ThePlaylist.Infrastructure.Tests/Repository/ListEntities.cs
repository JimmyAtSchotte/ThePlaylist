using FluentAssertions;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;
using ThePlaylist.Specifications;
using ThePlaylist.Specifications.Entitites.Playlist.Query;

namespace ThePlaylist.Infrastructure.Tests.Repository;

[TestFixture]
public class ListEntities
{
      
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void List(RepositorySource repositoryProvider)
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
    public async Task ListAsync(RepositorySource repositoryProvider)
    {
        var playlist = new Playlist()
        {
            Name = "My playlist 2",
            Description = "My playlist 2 description"
        };
        await using var repository = repositoryProvider.CreateRepository();
        
        await repository.AddAsync(playlist, CancellationToken.None);
        
        var playlists = await repository.ListAsync<Playlist>(CancellationToken.None);
        playlists.Should().Contain(x => x.Id == playlist.Id);
    }
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void ListBySpecification(RepositorySource repositoryProvider)
    {
        var playlist = new Playlist()
        {
            Name = Guid.NewGuid().ToString(),
            Description = "My playlist 2 description"
        };
        using var repository = repositoryProvider.CreateRepository();
        
        repository.Add(playlist);
        repository.List(Specs.Playlist.ByName(playlist.Name)).Should().Contain(x => x.Id == playlist.Id);
    }
    
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public async Task ListBySpecificationAsync(RepositorySource repositoryProvider)
    {
        var playlist = new Playlist()
        {
            Name = Guid.NewGuid().ToString(),
            Description = "My playlist 2 description"
        };
        await using var repository = repositoryProvider.CreateRepository();
        
        await repository.AddAsync(playlist, CancellationToken.None);
        (await repository.ListAsync(Specs.Playlist.ByName(playlist.Name), CancellationToken.None)).Should().Contain(x => x.Id == playlist.Id);
    }
}