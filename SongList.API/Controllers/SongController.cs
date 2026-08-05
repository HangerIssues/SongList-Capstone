using SongList.API.Models;
using Microsoft.AspNetCore.Mvc;
using SongList.API.Services;
using SongList.API.Dtos;
using System.Text.Json;

namespace SongList.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SongController : ControllerBase
{
    private List<Song> Songs { get;} = new List<Song>();

    public SongController()
    {
        string jsonFile = System.IO.File.ReadAllText("./Resources/SongList.json");
        var songData = JsonSerializer.Deserialize<List<Song>>(jsonFile);
        if (songData != null)
        {
            Songs = songData;
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<Song>>> GetAllSongs()
    {
        return Ok(Songs);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Song>> GetSongById(int id)
    {
        var song = Songs.FirstOrDefault(s => s.Id == id);

        if (song == null)
        {
            return NotFound();
        }

        return Ok(song);
    }

    [HttpPost]
    public async Task<ActionResult<Song>> AddSong(Song song)
    {
        var addedSong = new Song
        {
            Id = song.Id,
            Title = song.Title,
            Artist = song.Artist,
            Album = song.Album,
            Year = song.Year,
            Genre = song.Genre,
            Tags = song.Tags
        };
        return CreatedAtAction(nameof(GetSongById), new { id = addedSong.Id }, addedSong);
    }
/*
    [HttpPut("{id}")]
    public async Task<ActionResult<Song>> UpdateSong(int id, Song song)
    {
        var updatedSong = Songs.FirstOrDefault(s => s.Id == id);
        if(updatedSong == null)
        {
            return NotFound();
        }
        
        song.Id = updatedSong.Id;
        song.Title = updatedSong.Title;
        song.Artist = updatedSong.Artist;
        song.Album = updatedSong.Album;
        song.Year = updatedSong.Year;
        song.Genre = updatedSong.Genre;
        song.Tags = updatedSong.Tags;
    }
/*
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
    */
}