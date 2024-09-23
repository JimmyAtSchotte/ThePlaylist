using Microsoft.AspNetCore.Mvc;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Core.Interfaces;

namespace ThePlaylist.WebApi.Controllers;

[ApiController]
public class GenreController(IRepository repository) : Controller
{
    [HttpGet("api/genre")]
    public IActionResult List()
    {
        var genres = repository.List<Genre>();
        return Ok(genres);
    }
    
    [HttpPost("api/genre")]
    public IActionResult Add([FromBody] GenreAddCommand command)
    {
        var genre = new Genre()
        {
            Name = command.Name,
        };
        
        var result = repository.Add(genre);
        return Ok(result);
    }
    
    
    [HttpPost("api/genres")]
    public IActionResult AddGeneres([FromBody] GenreAddCommand[] commands)
    {
        var result = new List<Genre>();
        
        repository.ExecuteUnitOfWork(uow =>
        {
            foreach (var command in commands)
            {
                var genre = new Genre()
                {
                    Name = command.Name,
                };
                
                var g = uow.Add(genre);
                result.Add(g);
            }
        });
       
        return Ok(result.ToArray());
    }
}

public class GenreAddCommand
{
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
}