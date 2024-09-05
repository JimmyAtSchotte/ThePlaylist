using FluentAssertions;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Core.Interfaces;
using ThePlaylist.Infrastructure.NHibernate;
using ThePlaylist.Infrastructure.NHibernate.Mappings;
using ThePlaylist.Infrastructure.Tests.NHibernate.DatabaseConfigurations;

namespace ThePlaylist.Infrastructure.Tests.NHibernate;

[TestFixture]
public abstract class RepositoryTests
{
    private IRepository _repository;
    private ISession _session;
    protected abstract IDatabaseConfiguration DatabaseConfiguration { get; }

    [SetUp]
    public void Setup()
    {
        _session = DatabaseConfiguration.SessionFactory.OpenSession();
        new SchemaExport(DatabaseConfiguration.Configuration).Execute(true, true, false, _session.Connection, null);
        _repository = new Repository(_session);
    }

    [TearDown]
    public void TearDown()
    {
        _session.Dispose();
    }
    
    [Test]
    public void AddEntity()
    {
        var playlist = new Playlist()
        {
            Name = "My playlist",
            Description = "My playlist description"
        };

        var savesPlaylist = _repository.Add(playlist);
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

        _repository.Add(playlist);
        _session.Clear();
        
        var playlists = _repository.List<Playlist>();
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

        var savesPlaylist =_repository.Add(playlist);
        _repository.Delete(savesPlaylist);
        _session.Clear();
        
        var playlists = _repository.List<Playlist>();
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

        var savesPlaylist =_repository.Add(playlist);
        savesPlaylist.Description = "UPDATED";
        
        _repository.Update(savesPlaylist);
        _session.Clear();
        
        var playlists = _repository.List<Playlist>();
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

        _repository.Add(playlist);
        _session.Clear();
        
        var result = _repository.Get<Playlist>(playlist.Id);
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

        _repository.Add(playlistA);
        _session.Clear();
        _repository.Add(playlistB);
        _session.Clear();
        
        var fetchedPlaylistA = _repository.Get<Playlist>(playlistA.Id);
        var fetchedPlaylistB = _repository.Get<Playlist>(playlistB.Id);
    
        fetchedPlaylistA.Tracks.Should().HaveCount(2);
        fetchedPlaylistB.Tracks.Should().HaveCount(2);

        var fetchedTrackA = _repository.Get<Track>(trackA.Id);
        var fetchedTrackB = _repository.Get<Track>(trackB.Id);

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
        
        _repository.Add(rock);
        _session.Clear();

        var fetchedGenre = _repository.Get<Genre>(rock.Id);
        fetchedGenre.SubGenres.Should().HaveCount(1);
    }
}