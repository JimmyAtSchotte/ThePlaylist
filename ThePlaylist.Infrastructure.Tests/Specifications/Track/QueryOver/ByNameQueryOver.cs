using FluentAssertions;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;
using ThePlaylist.Specifications.Track.QueryOver;

namespace ThePlaylist.Infrastructure.Tests.Specifications.Track.QueryOver;

[TestFixture]
public class ByNameQueryOver
{
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.NHibernateOnlyRepositoryProviders))]
    public void FindTrackByName(BaseRepositorySource repositoryProvider)
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
        var fetchedTracks = repository.List(new TrackByNameQueryOver(track.Name));

        fetchedTracks.Should().Contain(x => x.Id == track.Id);
        fetchedTracks.Should().NotContain(x => x.Id == trackB.Id);
    }
}