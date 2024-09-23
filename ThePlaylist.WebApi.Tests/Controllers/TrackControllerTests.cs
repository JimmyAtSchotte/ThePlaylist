using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Core.Interfaces;
using ThePlaylist.WebApi.Controllers;

namespace ThePlaylist.WebApi.Tests.Controllers;

[TestFixture]
public class TrackControllerTests
{
    [Test]
    public void ListAll()
    {
        var expectedGenres = new List<Track>()
        {
            new Track()
        };
        
        var mock = new Mock<IRepository>();
        mock.Setup(x => x.List<Track>()).Returns(expectedGenres);

        var controller = new TrackController(mock.Object);
        var result = controller.List();
        
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(expectedGenres);
    }

    [Test]
    public void AddGenre()
    {
        var command = new TrackAddCommand()
        {
            Name = "Fantasy"
        };
        
        var mock = new Mock<IRepository>();
        mock.Setup(x => x.Add(It.IsAny<Track>())).Returns((Track track) => track);
        
        var controller = new TrackController(mock.Object);
        var result = controller.Add(command);
        
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        
        var responseObj = okResult!.Value as Track;
        responseObj.Should().NotBeNull();
        responseObj.Name.Should().Be(command.Name);
    }
    
}