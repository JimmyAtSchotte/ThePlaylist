using Microsoft.AspNetCore.Mvc;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Core.Interfaces;

namespace ThePlaylist.WebApi.Controllers;

[ApiController]
public class TrackController(IRepository repository) : Controller
{
    [HttpGet("api/track")]
    public IActionResult List()
    {
        var genres = repository.List<Track>();
        return Ok(genres);
    }
    
    [HttpPost("api/track")]
    public IActionResult Add([FromBody] TrackAddCommand command)
    {
        var track = new Track()
        {
            Name = command.Name,
        };
        
        var result = repository.Add(track);
        return Ok(result);
    }
}

public class TrackAddCommand
{
    public string Name { get; set; }
}