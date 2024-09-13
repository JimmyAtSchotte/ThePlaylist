using FluentAssertions;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;
using ThePlaylist.Specifications.Track.QueryOver;

namespace ThePlaylist.Infrastructure.Tests.Specifications.Track.QueryOver;

[TestFixture]
public class NamesQueryOverProjection
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
        
        var fetchedTracks = repository.List(new TrackNamesQueryOverProjection());
        
        fetchedTracks.Should().Contain(x => x.Name == track.Name);
    }
}