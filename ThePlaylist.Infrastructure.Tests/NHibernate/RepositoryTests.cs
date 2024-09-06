using FluentAssertions;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Core.Interfaces;
using ThePlaylist.Core.Specification;
using ThePlaylist.Core.Specification.Common.Genre;
using ThePlaylist.Core.Specification.Criterion;
using ThePlaylist.Core.Specification.Genre;

namespace ThePlaylist.Infrastructure.Tests.NHibernate;

[TestFixture]
public abstract class RepositoryTests
{
    protected abstract IRepository Repository { get; }
    

    [SetUp]
    public abstract void Setup();

    [TearDown]
    public abstract void TearDown();
    
    [Test]
    public void AddEntity()
    {
        var playlist = new Playlist()
        {
            Name = "My playlist",
            Description = "My playlist description"
        };

        var savesPlaylist = Repository.Add(playlist);
        savesPlaylist.Id.Should().NotBe(Guid.Empty);
    }
    
    [Test]
    public void ListEntities()
    {
        var playlist = new Playlist()
        {
            Name = "My playlist 2",
            Description = "My playlist 2 description"
        };

        Repository.Add(playlist);
        
        var playlists = Repository.List<Playlist>();
        playlists.Should().HaveCount(1);
    }
    
    [Test]
    public void DeleteEntity()
    {
        var playlist = new Playlist()
        {
            Name = "My playlist 3",
            Description = "My playlist 3 description"
        };

        var savesPlaylist =Repository.Add(playlist);
        Repository.Delete(savesPlaylist);
        
        var playlists = Repository.List<Playlist>();
        playlists.Should().NotContain(x => x.Id == savesPlaylist.Id);
    }
    
    [Test]
    public void UpdateEntity()
    {
        var playlist = new Playlist()
        {
            Name = "My playlist 4",
            Description = "My playlist 4 description"
        };

        var savesPlaylist =Repository.Add(playlist);
        savesPlaylist.Description = "UPDATED";
        
        Repository.Update(savesPlaylist);
        
        var playlists = Repository.List<Playlist>();
        playlists.First(x => x.Id == savesPlaylist.Id).Description.Should().Be("UPDATED");
    }
    
    [Test]
    public void GetEntity()
    {
        var playlist = new Playlist()
        {
            Name = "My playlist 5",
            Description = "My playlist 5 description"
        };

        Repository.Add(playlist);
        
        var result = Repository.Get<Playlist>(playlist.Id);
        result.Should().NotBeNull();
    }
    
    [Test]
    public void ManyToManyRelationship()
    {
        var rock = new Genre() { Name = "Rock" };
        var pop = new Genre() { Name = "Pop" };
        
        var trackA = new Track { Name = "Track A" };
        trackA.Genres.Add(rock);
        
        var trackB = new Track { Name = "Track B" };
        trackB.Genres.Add(pop);

        var playlistA = new Playlist { Name = "Playlist A" };
        var playlistB = new Playlist { Name = "Playlist B" };

        playlistA.AddTrack(trackA);
        playlistA.AddTrack(trackB);
    
        playlistB.AddTrack(trackA);
        playlistB.AddTrack(trackB);

        Repository.Add(playlistA);
        Repository.Add(playlistB);
        
        var fetchedPlaylistA = Repository.Get<Playlist>(playlistA.Id);
        var fetchedPlaylistB = Repository.Get<Playlist>(playlistB.Id);
    
        fetchedPlaylistA.Tracks.Should().HaveCount(2);
        fetchedPlaylistB.Tracks.Should().HaveCount(2);

        var fetchedTrackA = Repository.Get<Track>(trackA.Id);
        var fetchedTrackB = Repository.Get<Track>(trackB.Id);

        fetchedTrackA.Playlists.Should().HaveCount(2);
        fetchedTrackB.Playlists.Should().HaveCount(2);
        
        fetchedTrackA.Genres.Should().HaveCount(1);
        fetchedTrackB.Genres.Should().HaveCount(1);
    }
    
    [Test]
    public void SelfReferencingRelationship()
    {
        var rock = new Genre() { Name = "Rock" };
        var metal = new Genre() { Name = "metal" };
        rock.AddSubGenre(metal);
        
        Repository.Add(rock);

        var fetchedGenre = Repository.Get<Genre>(rock.Id);
        fetchedGenre.SubGenres.Should().HaveCount(1);
    }
    
    [Test]
    public void SpecificationByName()
    {
        var rock = new Genre() { Name = "Rock" };
        Repository.Add(rock);

        var specification = Specifications.Genre.ByName(rock.Name).Build();
        var fetchedGenres = Repository.List(specification).ToList();

        fetchedGenres.Should().HaveCount(1);
        fetchedGenres.First().Id.Should().Be(rock.Id);
    }
    
    [Test]
    public void SpecificationByName2()
    {
        var rock = new Genre() { Name = "Rock" };
        Repository.Add(rock);

        var specification = Specifications.Genre.ByName2(rock.Name).Build();
        var fetchedGenres = Repository.List(specification).ToList();

        fetchedGenres.Should().HaveCount(1);
        fetchedGenres.First().Id.Should().Be(rock.Id);
    }
    
    [Test]
    public void SpecificationCriteria()
    {
        var rock = new Genre() { Name = "Rock" };
        Repository.Add(rock);

        var specification = Specifications.Genre.Where(Restrictions.Eq("Name", rock.Name)).Build();
        var fetchedGenres = Repository.List(specification).ToList();

        fetchedGenres.Should().HaveCount(1);
        fetchedGenres.First().Id.Should().Be(rock.Id);
    }
}