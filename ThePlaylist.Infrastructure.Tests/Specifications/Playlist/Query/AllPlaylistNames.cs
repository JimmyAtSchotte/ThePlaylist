using FluentAssertions;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;
using ThePlaylist.Specifications;
using ThePlaylist.Specifications.Entitites.Playlist.Query;

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

        var names = repository.List(Specs.Playlist.AllPlaylistNames());
        names.Should().Contain(x => x.Name == playlist.Name);
    }
}