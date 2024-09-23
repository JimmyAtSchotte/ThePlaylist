using FluentAssertions;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Infrastructure.Exceptions;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;
using ThePlaylist.Specifications.Entitites.Genre.Query;
using ThePlaylist.Specifications.Entitites.Track.Query;

namespace ThePlaylist.Infrastructure.Tests.Repository;

[TestFixture]
public class UnitOfWork
{
     
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void RollbackOnAggregateRootError(RepositorySource repositoryProvider)
    {
        var trackA = new Track() { Name = Guid.NewGuid().ToString() };
        var genreA = trackA.AddGenre(new Genre() { Name = Guid.NewGuid().ToString() });
        
        var trackB = new Track() { Name = Guid.NewGuid().ToString() };
        trackB.AddGenre(new Genre() { Name = genreA.Name});
        
        using var repository = repositoryProvider.CreateRepository();
        repository.Add(trackA);

        repository.Invoking(r => r.Add(trackB)).Should().Throw<Exception>();
        repository.Invoking(r => r.Get(new TrackByName(trackB.Name))).Should().Throw<EntityNotFoundException>();
    }
    
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void RollbackUnitOfWork(RepositorySource repositoryProvider)
    {
        var genreA = new Genre() { Name = Guid.NewGuid().ToString() };
        var genreB = new Genre() { Name = genreA.Name };
        var genres = new List<Genre>() { genreA, genreB };
        
        using var repository = repositoryProvider.CreateRepository();

        try
        {
            repository.ExecuteUnitOfWork(r =>
            {
                foreach (var genre in genres)
                    r.Add(genre);
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        repository.Invoking(r => r.Get(new GenreByNameQuery(genreA.Name))).Should().Throw<EntityNotFoundException>();
    }
    
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public async Task RollbackUnitOfWorkAsync(RepositorySource repositoryProvider)
    {
        var genreA = new Genre() { Name = Guid.NewGuid().ToString() };
        var genreB = new Genre() { Name = genreA.Name };
        var genres = new List<Genre>() { genreA, genreB };

        await using var repository = repositoryProvider.CreateRepository();

        try
        {
            await repository.ExecuteUnitOfWorkAsync(async r =>
            {
                foreach (var genre in genres)
                   await r.AddAsync(genre, CancellationToken.None);
                
            }, CancellationToken.None);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        repository.Invoking(r => r.Get(new GenreByNameQuery(genreA.Name))).Should().Throw<EntityNotFoundException>();
    }
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void NestedUnitOfWorks(RepositorySource repositoryProvider)
    {
        var genre = new Genre() { Name = Guid.NewGuid().ToString() };
        var genreA = new Genre() { Name = Guid.NewGuid().ToString() };
        var genreB = new Genre() { Name = genreA.Name };
        var genres = new List<Genre>() { genreA, genreB };
        
        using var repository = repositoryProvider.CreateRepository();

        try
        {
            repository.ExecuteUnitOfWork(r =>
            {
                r.Add(genre);
            
                r.ExecuteUnitOfWork(r2 =>
                {
                    foreach (var g in genres)
                        r2.Add(g);
                });
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
      

        repository.Invoking(r => r.Get(new GenreByNameQuery(genre.Name))).Should().Throw<EntityNotFoundException>();
        repository.Invoking(r => r.Get(new GenreByNameQuery(genreA.Name))).Should().Throw<EntityNotFoundException>();
    }
    
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void ChainedUnitOfWorks(RepositorySource repositoryProvider)
    {
        var genre = new Genre() { Name = Guid.NewGuid().ToString() };
        var genreA = new Genre() { Name = Guid.NewGuid().ToString() };
        var genreB = new Genre() { Name = genreA.Name };
        var genres = new List<Genre>() { genreA, genreB };
        
        using var repository = repositoryProvider.CreateRepository();

        repository.ExecuteUnitOfWork(r =>
        {
            r.Add(genre);
        });

        try
        {
            repository.ExecuteUnitOfWork(r =>
            {
                foreach (var g in genres)
                    r.Add(g);
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
      

        repository.Invoking(r => r.Get(new GenreByNameQuery(genre.Name))).Should().NotThrow();
        repository.Invoking(r => r.Get(new GenreByNameQuery(genreA.Name))).Should().Throw<EntityNotFoundException>();
    }
}