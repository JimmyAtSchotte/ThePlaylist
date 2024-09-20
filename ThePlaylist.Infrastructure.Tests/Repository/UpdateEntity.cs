using FluentAssertions;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;

namespace ThePlaylist.Infrastructure.Tests.Repository;

[TestFixture]
public class UpdateEntity
{
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void Update(RepositorySource repositoryProvider)
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
    public async Task UpdateAsync(RepositorySource repositoryProvider)
    {
        var playlist = new Playlist()
        {
            Name = "My playlist 4",
            Description = "My playlist 4 description"
        };

        await using var repository = repositoryProvider.CreateRepository();
        var savesPlaylist = await repository.AddAsync(playlist, CancellationToken.None);
        savesPlaylist.Description = "UPDATED";
        
        await repository.UpdateAsync(savesPlaylist, CancellationToken.None);
        
        var playlists = await repository.ListAsync<Playlist>(CancellationToken.None);
        playlists.First(x => x.Id == savesPlaylist.Id).Description.Should().Be("UPDATED");
    }
}