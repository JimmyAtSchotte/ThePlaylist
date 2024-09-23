using FluentAssertions;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;
using ThePlaylist.Specifications;
using ThePlaylist.Specifications.Entitites.Playlist.Query;


namespace ThePlaylist.Infrastructure.Tests.Specifications.Playlist.Query;

[TestFixture]
public class AllTracksAllGenresTest
{
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void IncludeAllTrackIncludeAllGenres(RepositorySource repositorySource)
    {
        var playlist = new Core.Entitites.Playlist()
        {
            Name = "TestPlaylist",
            Description = "TestPlaylistDescription",
        };
        
        var track = playlist.AddTrack(new Core.Entitites.Track()
        {
            Name = "TestTrack",
        });
        
        track.AddGenre(new Genre(){Name = Guid.NewGuid().ToString()});

        using var repository = repositorySource.CreateRepository();
        repository.Add(playlist);

        var fetchedPlaylists = repository.List(Specs.Playlist.AllTracksAllGenres());
        var fetchedPlaylist = fetchedPlaylists.FirstOrDefault(x => x.Id == playlist.Id);

        fetchedPlaylist.Tracks.Should().HaveCount(1);
        fetchedPlaylist.Tracks.SelectMany(x => x.Genres).Should().HaveCount(1);
    }
}

