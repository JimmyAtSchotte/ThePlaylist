using FluentAssertions;
using ThePlaylist.Specifications.Track;

namespace ThePlaylist.Infrastructure.Tests.NHibernate.Specifications.Track;

[TestFixture]
public class NamesCriteriaProjection
{
    [TestCaseSource(typeof(NHibernateRepositorySources), nameof(NHibernateRepositorySources.RepositoryProviders))]
    public void TrackNames(BaseNHibernateRepositorySource repositoryProvider)
    {
        var track = new Core.Entitites.Track
        {
            Name = Guid.NewGuid().ToString()
        };
        
        using var repository = repositoryProvider.CreateRepository();
        repository.Add(track);
        
        var fetchedTracks = repository.List(new TrackNamesCriteriaProjection());
        
        fetchedTracks.Should().Contain(x => x.Name == track.Name);
    }
}