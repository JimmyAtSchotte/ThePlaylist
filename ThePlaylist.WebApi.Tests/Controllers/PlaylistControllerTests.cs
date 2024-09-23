using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Core.Interfaces;
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