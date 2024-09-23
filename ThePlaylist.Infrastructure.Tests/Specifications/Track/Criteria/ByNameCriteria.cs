using FluentAssertions;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;
using ThePlaylist.Specifications.Entitites.Track.Criteria;

namespace ThePlaylist.Infrastructure.Tests.Specifications.Track.Criteria;

[TestFixture]
public class ByNameCriteria
{
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.NHibernateOnlyRepositoryProviders))]
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
        var fetchedTracks = repository.List(new TrackByNameCriteria(track.Name));
        
        fetchedTracks.Should().Contain(x => x.Id == track.Id);
        fetchedTracks.Should().NotContain(x => x.Id == trackB.Id);
    }
}