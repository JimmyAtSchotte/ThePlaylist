﻿using Ardalis.Specification;
using FluentAssertions;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Core.Interfaces;
using ThePlaylist.Core.Specification;
using ThePlaylist.Core.Specification.Common.Genre;
using ThePlaylist.Core.Specification.Criterion;

namespace ThePlaylist.Infrastructure.Tests;

[TestFixture]
public abstract class BaseRepositoryTests
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
        playlists.Should().Contain(x => x.Id == playlist.Id);
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
        trackA.AddGenre(rock);
        
        var trackB = new Track { Name = "Track B" };
        trackB.AddGenre(pop);

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
        var pop = new Genre() { Name = "Pop" };
        Repository.Add(rock);
        Repository.Add(pop);


        var specification = Specifications.Genre.ByName(rock.Name).Specification;
        var fetchedGenres = Repository.List(specification).ToList();

        fetchedGenres.Should().Contain(x => x.Id == rock.Id);
        fetchedGenres.Should().NotContain(x => x.Id == pop.Id);
    }
    
    [Test]
    public void SpecificationExpression()
    {
        var rock = new Genre() { Name = "Rock" };
        var pop = new Genre() { Name = "Pop" };
        Repository.Add(rock);
        Repository.Add(pop);

        var specification = Specifications.Genre.Where(x => x.Name == rock.Name).Specification;
        var fetchedGenres = Repository.List(specification).ToList();

        fetchedGenres.Should().Contain(x => x.Id == rock.Id);
        fetchedGenres.Should().NotContain(x => x.Id == pop.Id);
    }
    
    [Test]
    public void SpecificationCriteria()
    {
        var rock = new Genre() { Name = "Rock" };
        var pop = new Genre() { Name = "Pop" };
        Repository.Add(rock);
        Repository.Add(pop);

        var specification = Specifications.Genre.Where(Restrictions.Eq("Name", rock.Name)).Specification;
        var fetchedGenres = Repository.List(specification).ToList();

        fetchedGenres.Should().Contain(x => x.Id == rock.Id);
        fetchedGenres.Should().NotContain(x => x.Id == pop.Id);
    }
    
    
    [Test]
    public void SpecificationIncludeGenres()
    {
        var trackA = new Track() { Name = "Track A" };
        var rock = new Genre() { Name = "Rock" };
        trackA.AddGenre(rock);
        
        var trackB = new Track() { Name = "Track B" };
        trackB.AddGenre(rock);
        
        Repository.Add(trackA);
        Repository.Add(trackB);
        
        var specification = Specifications.Track.Include(x => x.Genres).Specification;
        var fetchedTracks = Repository.List(specification).ToList();

        fetchedTracks.Should().Contain(x => x.Id == trackA.Id);
        fetchedTracks.FirstOrDefault(x => x.Id == trackA.Id).Genres.Should().NotBeEmpty();
        fetchedTracks.Should().Contain(x => x.Id == trackB.Id);
        fetchedTracks.FirstOrDefault(x => x.Id == trackB.Id).Genres.Should().NotBeEmpty();

        
    }
}