using FluentAssertions;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;
using ThePlaylist.Specifications.Entitites.Track.Query;

namespace ThePlaylist.Infrastructure.Tests.Repository;

[TestFixture]
public class Projections
{
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void Projection(RepositorySource repositoryProvider)
    {
        var track = new Core.Entitites.Track
        {
            Name = Guid.NewGuid().ToString()
        };
        
        using var repository = repositoryProvider.CreateRepository();
        repository.Add(track);
        
        repository.List(new TrackByNameProjection(track.Name)).Should().Contain(x => x.Name == track.Name);
    }
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public async Task ProjectionAsync(RepositorySource repositoryProvider)
    {
        var track = new Core.Entitites.Track
        {
            Name = Guid.NewGuid().ToString()
        };
        
        await using var repository = repositoryProvider.CreateRepository();
        await repository.AddAsync(track, CancellationToken.None);
        (await repository.ListAsync(new TrackByNameProjection(track.Name), CancellationToken.None)).Should().Contain(x => x.Name == track.Name);
    }
}