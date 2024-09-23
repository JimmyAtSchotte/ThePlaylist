using FluentAssertions;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;
using ThePlaylist.Specifications.Entitites.Track.HQL;

namespace ThePlaylist.Infrastructure.Tests.Specifications.Track.HQL;

[TestFixture]
public class NamesHqlProjection
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
        
        repository.List(new TrackNamesHqlProjection()).Should().Contain(x => x.Name == track.Name);
    }
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.NHibernateOnlyRepositoryProviders))]
    public void TrackNamesTransformerProjection(RepositorySource repositoryProvider)
    {
        var track = new Core.Entitites.Track
        {
            Name = Guid.NewGuid().ToString()
        };
        
        using var repository = repositoryProvider.CreateRepository();
        repository.Add(track);
        
        var fetchedTracks = repository.List(new TrackNamesHqlTransformerProjection());
        
        fetchedTracks.Should().Contain(x => x.Name == track.Name);
    }
}