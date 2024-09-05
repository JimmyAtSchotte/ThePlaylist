using Microsoft.AspNetCore.Mvc;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Core.Interfaces;

namespace ThePlaylist.WebApi.Controllers;

[ApiController]
public class PlaylistController(IRepository repository) : Controller
{
    [HttpGet("api/playlist")]
    public IActionResult List()
    {
        var genres = repository.List<Playlist>();
        return Ok(genres);
    }
    
    [HttpPost("api/playlist")]
    public IActionResult Add([FromBody] TrackAddCommand command)
    {
        var playlist = new Playlist()
        {
            Name = command.Name,
        };
        
        var result = repository.Add(playlist);
        return Ok(result);
    }
}

public class PlaylistAddCommand
{
    public string Name { get; set; }
}