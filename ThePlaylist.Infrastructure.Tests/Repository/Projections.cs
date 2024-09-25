using FluentAssertions;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;
using ThePlaylist.Specifications;
using ThePlaylist.Specifications.Entitites.Track.Query;

namespace ThePlaylist.Infrastructure.Tests.Repository;

[TestFixture]
public class Projections
{
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void Projection(IRepositorySource repositoryProvider)
    {
        var track = new Core.Entitites.Track
        {
            Name = Guid.NewGuid().ToString()
        };
        
        using var repository = repositoryProvider.CreateRepository();
        repository.Add(track);
        
        repository.List(Specs.Track.TrackByNameProjection(track.Name)).Should().Contain(x => x.Name == track.Name);
    }
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public async Task ProjectionAsync(IRepositorySource repositoryProvider)
    {
        var track = new Core.Entitites.Track
        {
            Name = Guid.NewGuid().ToString()
        };
        
        await using var repository = repositoryProvider.CreateRepository();
        await repository.AddAsync(track, CancellationToken.None);
        (await repository.ListAsync(Specs.Track.TrackByNameProjection(track.Name), CancellationToken.None)).Should().Contain(x => x.Name == track.Name);
    }
}