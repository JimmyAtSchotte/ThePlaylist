using FluentAssertions;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Infrastructure.Exceptions;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;

namespace ThePlaylist.Infrastructure.Tests.Repository;

[TestFixture]
public class Exceptions
{
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void ThrowsEntityNotFound(IRepositorySource repositoryProvider)
    {
        using var repository = repositoryProvider.CreateRepository();
        repository.Invoking(r => r.Get<Playlist>(Guid.NewGuid())).Should().Throw<EntityNotFoundException>();
    }
    

}