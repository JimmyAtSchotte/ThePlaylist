using FluentAssertions;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;
using ThePlaylist.Specifications;
using ThePlaylist.Specifications.Entitites.Track.Query;

namespace ThePlaylist.Infrastructure.Tests.Projections;

[TestFixture]
public class AnonymousProjection
{
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void TrackByName(IRepositorySource repositoryProvider)
    {
        var track = new Core.Entitites.Track
        {
            Name = Guid.NewGuid().ToString()
        };
        
        using var repository = repositoryProvider.CreateRepository();
        repository.Add(track);

        var specification = Specs.Track.TrackByName(track.Name,
        x => new 
        {
            x.Name
        });
        
        var list = repository.List(specification);
        
        list.Should().Contain(x => x.Name == track.Name);
    }
}