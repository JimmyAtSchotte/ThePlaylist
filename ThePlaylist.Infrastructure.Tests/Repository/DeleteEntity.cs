using FluentAssertions;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;

namespace ThePlaylist.Infrastructure.Tests.Repository;

[TestFixture]
public class DeleteEntity
{
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void Delete(RepositorySource repositoryProvider)
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
    public async Task DeleteAsync(RepositorySource repositoryProvider)
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

}