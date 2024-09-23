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
            Name = "Pop"
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
    
    
    [Test]
    public void UnitOfWork()
    {
        var command1 = new GenreAddCommand()
        {
            Name = "Pop"
        };
        
        var command2 = new GenreAddCommand()
        {
            Name = "Rock"
        };
        
        var mock = new Mock<IRepository>();
        mock.Setup(x => x.Add(It.IsAny<Genre>())).Returns((Genre genre) => genre);
        mock.Setup(x => x.ExecuteUnitOfWork(It.IsAny<Action<IRepository>>()))
            .Callback((Action<IRepository> action) => action(mock.Object));
        
        var controller = new GenreController(mock.Object);
        var result = controller.AddGeneres([command1, command2]);
        
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        
        var responseObj = okResult!.Value as Genre[];
        responseObj.Should().NotBeNull();
        responseObj.Should().Contain(x => x.Name == command1.Name);
        responseObj.Should().Contain(x => x.Name == command2.Name);
    }
}