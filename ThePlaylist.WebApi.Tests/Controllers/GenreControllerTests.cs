using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Core.Interfaces;
using ThePlaylist.WebApi.Controllers;

namespace ThePlaylist.WebApi.Tests.Controllers;

[TestFixture]
public class GenreControllerTests
{
    [Test]
    public void ListAll()
    {
        var expectedGenres = new List<Genre>()
        {
            new Genre()
        };
        
        var mock = new Mock<IRepository>();
        mock.Setup(x => x.List<Genre>()).Returns(expectedGenres);

        var controller = new GenreController(mock.Object);
        var result = controller.List();
        
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(expectedGenres);
    }

    [Test]
    public void AddGenre()
    {
        var command = new GenreAddCommand()
        {
            Name = "Fantasy"
        };
        
        var mock = new Mock<IRepository>();
        mock.Setup(x => x.Add(It.IsAny<Genre>())).Returns((Genre genre) => genre);
        
        var controller = new GenreController(mock.Object);
        var result = controller.Add(command);
        
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        
        var responseObj = okResult!.Value as Genre;
        responseObj.Should().NotBeNull();
        responseObj.Name.Should().Be(command.Name);
    }
    
}