using System.Collections;
using FluentAssertions;
using ThePlaylist.Specifications.Track;

namespace ThePlaylist.Infrastructure.Tests.NHibernate.Specifications.Track;

[TestFixture]
public class ByNameCriteria
{
    [TestCaseSource(typeof(NHibernateRepositorySources), nameof(NHibernateRepositorySources.RepositoryProviders))]
    public void FindTrackByName(BaseNHibernateRepositorySource repositoryProvider)
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