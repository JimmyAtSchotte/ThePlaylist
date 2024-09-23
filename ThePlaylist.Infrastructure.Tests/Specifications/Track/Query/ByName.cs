using FluentAssertions;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;
using ThePlaylist.Specifications.Entitites.Track.Query;

namespace ThePlaylist.Infrastructure.Tests.Specifications.Track.Query;

[TestFixture]
public class ByName
{
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void FindTrackByName(RepositorySource repositoryProvider)
    {
        var track = new Core.Entitites.Track
        {
            Name = "Test"
        };
        
        var trackB = new Core.Entitites.Track
        {
            Name = "Test B"
        };

        using var repository = repositoryProvider.CreateRepository();
        repository.Add(track);
        repository.Add(trackB);

        var fetchedTracks = repository.List(new TrackByName(track.Name));
        
        fetchedTracks.Should().Contain(x => x.Id == track.Id);
        fetchedTracks.Should().NotContain(x => x.Id == trackB.Id);
    }
}