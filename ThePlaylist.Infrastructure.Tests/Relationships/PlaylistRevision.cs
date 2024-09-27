using Ardalis.Specification;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Core.Interfaces;
using ThePlaylist.Infrastructure.EntityFramework;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;
using ThePlaylist.Specifications;

namespace ThePlaylist.Infrastructure.Tests.Relationships;

[TestFixture]
public class PlaylistRevision
{
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void CreateNewRevision(IRepositorySource repositorySource)
    {
        var playlist = new Playlist()
        {
            Name = "TestPlaylist",
        };

        using var repository = repositorySource.CreateRepository(playlist);
        var addRevision = repository.Get(Specs.ById<Playlist>(playlist.Id));
        
        addRevision.CreateRevision(current => current.Name = "UpdatedPlaylist");
        repository.Update(addRevision);
        
        var updatedRevision = repository.Get(Specs.ById<Playlist>(playlist.Id, query => query.Include(x => x.Revisions)));
        
        updatedRevision.Revisions.Should().HaveCount(1);
    }
}