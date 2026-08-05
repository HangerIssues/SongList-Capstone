using SongList.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using SongList.API.Data;
using Microsoft.EntityFrameworkCore;
using SongList.API.Dtos;

namespace SongList.API.Services;

public class SongListService(ApplicationDbContext context) : ISongListService
{
/*
    var jsonFile = File.ReadAllText("./Resources/SongList.json");
    var jsonData = JsonSerializer.Deserialize<List<Song>>(
        json: jsonFile,
        options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
*/
    public async Task<List<GetSongDto>> GetAllSongs()
    {
        var results = await context.Songs.ToListAsync();
        return results.Select(s => new GetSongDto
        {
            Title = s.Title,
            Artist = s.Artist
        }).ToList();
    }

    public async Task<GetSongDto> GetSongById(int id)
    {
        var songs = await GetAllSongs();
        return songs.FirstOrDefault(s => s.Id == id);
    }

    public async Task<List<GetSongDto>> GetSongsByTitle(string title)
    {
        var songs = await GetAllSongs();
        return songs.Where(s => s.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public async Task<List<GetSongDto>> GetSongsByArtist(string artist)
    {
        var songs = await GetAllSongs();
        return songs.Where(s => s.Artist.Contains(artist, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public async Task<List<GetSongDto>> GetSongsByAlbum(string album)
    {
        var songs = await GetAllSongs();
        return songs.Where(s => s.Album.Contains(album, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public async Task<List<GetSongDto>> GetSongsByYear(int year)
    {
        var songs = await GetAllSongs();
        return songs.Where(s => s.Year == year).ToList();
    }

    public async Task<List<GetSongDto>> GetSongsByGenre(string genre)
    {
        var songs = await GetAllSongs();
        return songs.Where(s => s.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public async Task<List<GetSongDto>> GetSongsByTag(string tag)
    {
        var songs = await GetAllSongs();
        return songs.Where(s => s.Tags.Contains(tag, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public async Task<PostSongDto> AddSong(Song song)
    {
        var songs = await GetAllSongs();
        song.Id = songs.Max(s => s.Id) + 1;
        songs.Add(song);
        // In a real application, you would save the updated list back to the JSON file
        return song;
    }

    public async Task<bool> UpdateSong(int id, Song song)
    {
        var existingSong = await GetSongById(id);
        if (existingSong == null) return false;

        var songs = await GetAllSongs();
        songs.Remove(existingSong);
        song.Id = id;
        songs.Add(song);
        // In a real application, you would save the updated list back to the JSON file
        return true;
    }

    public async Task<bool> DeleteSong(int id)
    {
        var existingSong = await GetSongById(id);
        if (existingSong == null) return false;

        var songs = await GetAllSongs();
        songs.Remove(existingSong);
        // In a real application, you would save the updated list back to the JSON file
        return true;
    }
}