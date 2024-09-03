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
        var trackA = new Track { Name = "Track A" };
        var trackB = new Track { Name = "Track B" };

        var playlistA = new Playlist { Name = "Playlist A" };
        var playlistB = new Playlist { Name = "Playlist B" };

        playlistA.Tracks.Add(trackA);
        playlistA.Tracks.Add(trackB);
    
        playlistB.Tracks.Add(trackA);
        playlistB.Tracks.Add(trackB);

        _repository.Add(playlistA);
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
    }
}