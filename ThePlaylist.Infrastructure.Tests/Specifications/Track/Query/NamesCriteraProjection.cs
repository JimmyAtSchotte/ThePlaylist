using FluentAssertions;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;
using ThePlaylist.Specifications.Track.Criteria;

namespace ThePlaylist.Infrastructure.Tests.Specifications.Track.Query;

[TestFixture]
public class NamesCriteriaProjection
{
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.NHibernateOnlyRepositoryProviders))]
    public void TrackNames(BaseRepositorySource repositoryProvider)
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