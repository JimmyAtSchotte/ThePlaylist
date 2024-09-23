using FluentAssertions;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;
using ThePlaylist.Specifications;
using ThePlaylist.Specifications.Entitites.Track.QueryOver;

namespace ThePlaylist.Infrastructure.Tests.Specifications.Track.QueryOver;

[TestFixture]
public class NamesQueryOverProjection
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
        
        var fetchedTracks = repository.List(Specs.Track.TrackNamesQueryOverProjection());
        
        fetchedTracks.Should().Contain(x => x.Name == track.Name);
    }
}