using FluentAssertions;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;
using ThePlaylist.Specifications;

namespace ThePlaylist.Infrastructure.Tests.Repository;

[TestFixture]
public class GetEntity
{
       
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void Get(IRepositorySource repositoryProvider)
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
    public async Task GetAsync(IRepositorySource repositoryProvider)
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
    
    [Ignore("NHibernate keeps lazy loading even when trying to disable lazy loading.")]
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void GetNoLazyLoading(IRepositorySource repositoryProvider)
    {
        var playlist = new Playlist()
        {
            Name = "My playlist 2",
            Description = "My playlist 2 description"
        };
        playlist.AddTrack(new Track()
        {
            Name = "My track 2",
        });
        
        using (var repository = repositoryProvider.CreateRepository())
            repository.Add(playlist);

        Playlist fetchedPlaylist;
        
        using (var repository2 = repositoryProvider.CreateRepository())
            fetchedPlaylist = repository2.Get<Playlist>(playlist.Id);
        
        fetchedPlaylist.Tracks.Should().HaveCount(0);
    }
}