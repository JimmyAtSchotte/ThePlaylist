using FluentAssertions;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;
using ThePlaylist.Specifications;
using ThePlaylist.Specifications.Entitites.Track.Query;

namespace ThePlaylist.Infrastructure.Tests.Projections;

[TestFixture]
public class TrackName
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

        var specification = Specs.Track.TrackByName<Core.Projections.TrackName>(track.Name,
            x => new Core.Projections.TrackName()
            {
                Name = x.Name
            });
        
        var list = repository.List(specification);
        
        list.Should().Contain(x => x.Name == track.Name);
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
        
        
        var specification = Specs.Track.TrackByName<Core.Projections.TrackName>(track.Name,
        x => new Core.Projections.TrackName()
        {
            Name = x.Name
        });
        
        var list = await repository.ListAsync(specification, CancellationToken.None);
        
        list.Should().Contain(x => x.Name == track.Name);
    }
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void AnnonumusProjection(IRepositorySource repositoryProvider)
    {
        var track = new Core.Entitites.Track
        {
            Name = Guid.NewGuid().ToString()
        };
        
        using var repository = repositoryProvider.CreateRepository();
        repository.Add(track);

        var specification = Specs.Track.TrackByName<Core.Projections.TrackName>(track.Name,
        x => new ()
        {
            Name = x.Name
        });
        
        var list = repository.List(specification);
        
        list.Should().Contain(x => x.Name == track.Name);
    }
}