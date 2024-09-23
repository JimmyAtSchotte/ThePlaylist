using Microsoft.AspNetCore.Mvc;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Core.Interfaces;
using ThePlaylist.Specifications;
using ThePlaylist.Specifications.Entitites.Playlist.Query;

namespace ThePlaylist.WebApi.Controllers;

[ApiController]
public class PlaylistController(IRepository repository) : Controller
{
    [HttpGet("api/playlist")]
    public IActionResult List()
    {
        var playlists = repository.List<Playlist>();
        return Ok(playlists);
    }
    
    [HttpPost("api/playlist")]
    public IActionResult Add([FromBody] PlaylistAddCommand command)
    {
        var playlist = new Playlist()
        {
            Name = command.Name,
        };
        
        var result = repository.Add(playlist);
        return Ok(result);
    }

    [HttpGet("api/playlist")]
    public IActionResult List(PlaylistQuery query)
    {
        var playlists = repository.List<Playlist>(Specs.Playlist.ByName(query.Name));
        return Ok(playlists);
    }
}

public class PlaylistAddCommand
{
    public string Name { get; set; }
}

public class PlaylistQuery
{
    public string Name { get; set; }
}