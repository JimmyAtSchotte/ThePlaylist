using FluentAssertions;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;
using ThePlaylist.Specifications;
using ThePlaylist.Specifications.Entitites.Track.Criteria;

namespace ThePlaylist.Infrastructure.Tests.Specifications.Track.Query;

[TestFixture]
public class NamesCriteriaProjection
{
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.NHibernateOnlyRepositoryProviders))]
    public void TrackNames(RepositorySource repositoryProvider)
    {
        var track = new Core.Entitites.Track
        {
            Name = Guid.NewGuid().ToString()
        };
        
        using var repository = repositoryProvider.CreateRepository();
        repository.Add(track);
        
        var fetchedTracks = repository.List(Specs.Track.TrackNamesCriteriaProjection());
        
        fetchedTracks.Should().Contain(x => x.Name == track.Name);
    }
}