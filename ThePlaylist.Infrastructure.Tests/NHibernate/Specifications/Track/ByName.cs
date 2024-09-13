using System.Collections;
using FluentAssertions;

namespace ThePlaylist.Infrastructure.Tests.NHibernate.Specifications.Track;

[TestFixture]
public class ByName
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

        var fetchedTracks = repository.List(new TrackByName(track.Name));
        
        fetchedTracks.Should().Contain(x => x.Id == track.Id);
        fetchedTracks.Should().NotContain(x => x.Id == trackB.Id);
    }
}