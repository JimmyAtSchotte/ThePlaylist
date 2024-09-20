using FluentAssertions;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;
using ThePlaylist.Specifications;

namespace ThePlaylist.Infrastructure.Tests.Repository;

[TestFixture]
public class GetEntity
{
       
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void Get(RepositorySource repositoryProvider)
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
    public async Task GetAsync(RepositorySource repositoryProvider)
    {
        var playlist = new Playlist()
        {
            Name = "My playlist 5",
            Description = "My playlist 5 description"
        };

        await using var repository = repositoryProvider.CreateRepository();
        await repository.AddAsync(playlist, CancellationToken.None);
        
        (await repository.GetAsync<Playlist>(playlist.Id, CancellationToken.None)).Should().NotBeNull();
        (await repository.GetAsync<Playlist>(new ById<Playlist>(playlist.Id), CancellationToken.None)).Should().NotBeNull();
    }
}