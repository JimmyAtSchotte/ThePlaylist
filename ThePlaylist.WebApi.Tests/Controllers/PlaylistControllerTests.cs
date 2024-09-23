using Ardalis.Specification;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Core.Interfaces;
using ThePlaylist.Specifications;
using ThePlaylist.Specifications.Entitites.Playlist.Query;
using ThePlaylist.WebApi.Controllers;

namespace ThePlaylist.WebApi.Tests.Controllers;

[TestFixture]
public class PlaylistControllerTests
{
    [Test]
    public void ListAll()
    {
        var expectedGenres = new List<Playlist>()
        {
            new Playlist()
        };
        
        var mock = new Mock<IRepository>();
        mock.Setup(x => x.List<Playlist>()).Returns(expectedGenres);

        var controller = new PlaylistController(mock.Object);
        var result = controller.List();
        
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(expectedGenres);
    }
    
    
    [Test]
    public void ListQueryByNames()
    {
        var playlist = new Playlist()
        {
            Name = "Test1"
        };
        var playlists = new List<Playlist>()
        {
            playlist
        };
        
        var mock = new Mock<IRepository>();
        mock.Setup(x => x.List(It.IsAny<ISpecification<Playlist>>())).Returns(new List<Playlist>());
        mock.Setup(x => x.List(It.Is<ISpecification<Playlist>>(spec => spec.Equals(Specs.Playlist.ByName(playlist.Name))))).Returns(playlists);

        var controller = new PlaylistController(mock.Object);
        var result = controller.List(new PlaylistQuery()
        {
            Name = playlist.Name
        });
        
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(playlists);
    }

    [Test]
    public void AddGenre()
    {
        var command = new PlaylistAddCommand()
        {
            Name = "Fantasy"
        };
        
        var mock = new Mock<IRepository>();
        mock.Setup(x => x.Add(It.IsAny<Playlist>())).Returns((Playlist genre) => genre);
        
        var controller = new PlaylistController(mock.Object);
        var result = controller.Add(command);
        
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        
        var responseObj = okResult!.Value as Playlist;
        responseObj.Should().NotBeNull();
        responseObj.Name.Should().Be(command.Name);
    }
  
}

