using FluentAssertions;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;

namespace ThePlaylist.Infrastructure.Tests.Specifications.Playlist.Query;

[TestFixture]
public class AllPlaylistNames
{
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void Select(RepositorySource repositoryProvider)
    {
        var playlist = new Core.Entitites.Playlist() { Name = "My playlist" };
      
        using var repository = repositoryProvider.CreateRepository();
        repository.Add(playlist);

        var names = repository.List(new ThePlaylist.Specifications.Entitites.Playlist.Query.AllPlaylistNames());
        names.Should().Contain(x => x.Name == playlist.Name);
    }
}