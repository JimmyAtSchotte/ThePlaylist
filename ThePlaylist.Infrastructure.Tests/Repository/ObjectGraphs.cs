using Ardalis.Specification;
using FluentAssertions;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;
using ThePlaylist.Specifications;

namespace ThePlaylist.Infrastructure.Tests.Repository;

[TestFixture]
public class ObjectGraphs
{
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.RepositoryProviders))]
    public void AddEntityFullGraph(IRepositorySource repositoryProvider)
    {
        var rock = new Genre() { Name = Guid.NewGuid().ToString() };
        var pop = new Genre() { Name = Guid.NewGuid().ToString() };
        
        var trackA = new Track { Name = "Track A" };
        trackA.AddGenre(rock);
        
        var trackB = new Track { Name = "Track B" };
        trackB.AddGenre(pop);

        var playlistA = new Playlist { Name = "Playlist A" };
        
        using var repository = repositoryProvider.CreateRepository();
        playlistA.AddTrack(trackA);
        playlistA.AddTrack(trackB);
        repository.Add(playlistA);
        
        var reloadedPlaylist = repository.Get(Specs.ById<Playlist>(playlistA.Id, query => query.Include(x => x.Tracks).ThenInclude(x => x.Genres)));
        
        reloadedPlaylist.Tracks.Should().HaveCount(2);
        reloadedPlaylist.Tracks.FirstOrDefault(x => x.Id == trackA.Id)?.Genres.Should().Contain(x => x.Id == rock.Id);
        reloadedPlaylist.Tracks.FirstOrDefault(x => x.Id == trackB.Id)?.Genres.Should().Contain(x => x.Id == pop.Id);
        reloadedPlaylist.Tracks.FirstOrDefault(x => x.Id == trackA.Id)?.Playlists.Should().HaveCount(1);
        reloadedPlaylist.Tracks.FirstOrDefault(x => x.Id == trackB.Id)?.Playlists.Should().HaveCount(1);
    }
    

}