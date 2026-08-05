using SongList.API.Models;
using Microsoft.AspNetCore.Mvc;
using SongList.API.Services;
using SongList.API.Dtos;

namespace SongList.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SongController(ISongListService songListService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<GetSongDto>>> GetAllSongs()
    {
        var songs = await songListService.GetAllSongs();
        return Ok(songs);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetSongDto>> GetSongById(int id)
    {
        var song = await songListService.GetSongById(id);
        if (song == null)
        {
            return NotFound();
        }
        return Ok(song);
    }

    [HttpPost]
    public async Task<ActionResult<PostSongDto>> AddSong(Song song)
    {
        var addedSong = await songListService.AddSong(song);
        return CreatedAtAction(nameof(GetSongById), new { id = addedSong.Id }, addedSong);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PutSongDto>> UpdateSong(int id, Song song)
    {
        var updatedSong = await songListService.UpdateSong(id, song);
        if (updatedSong == null)
        {
            return NotFound();
        }
        return Ok(updatedSong);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSong(int id)
    {
        var deleted = await songListService.DeleteSong(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}