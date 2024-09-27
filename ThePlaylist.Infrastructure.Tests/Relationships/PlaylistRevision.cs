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
        playlist.CreateRevision(current => current.Name = "UpdatedPlaylist 1");

        using var repository = repositorySource.CreateRepository(playlist);
        var entity = repository.Get(Specs.ById<Playlist>(playlist.Id));
        entity.CreateRevision(current => current.Name = "UpdatedPlaylist 2");
        repository.Add(entity);
        
        var updatedEntity = repository.Get(Specs.ById<Playlist>(playlist.Id, query => query.Include(x => x.Revisions)));
        
        updatedEntity.Revisions.Should().HaveCount(2);
        updatedEntity.Revisions.First().Id.Should().NotBe(Guid.Empty);
    }
}