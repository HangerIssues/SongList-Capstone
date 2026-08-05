/*using SongList.API.Models;
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
        var song = await context.Songs.FindAsync(id);
        if (song == null)
        {
            return null; // Good job computer. Null is in fact a possibly null reference.
        }
        return new GetSongDto
        {
            Id = song.Id,
            Title = song.Title,
            Artist = song.Artist
        };
    }

    public async Task<List<GetSongDto>> GetSongsByTitle(string title)
    {
        var songs = await context.Songs
            .Where(s => s.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
            .ToListAsync();
        return songs.Select(s => new GetSongDto
        {
            Title = s.Title,
            Artist = s.Artist
        }).ToList();
    }

    public async Task<List<GetSongDto>> GetSongsByArtist(string artist)
    {
        var songs = await context.Songs
            .Where(s => s.Artist.Contains(artist, StringComparison.OrdinalIgnoreCase))
            .ToListAsync();
        return songs.Select(s => new GetSongDto
        {
            Title = s.Title,
            Artist = s.Artist
        }).ToList();
    }

    public async Task<List<GetSongDto>> GetSongsByAlbum(string album)
    {
        var songs = await context.Songs
            .Where(s => s.Album.Contains(album, StringComparison.OrdinalIgnoreCase))
            .ToListAsync();
        return songs.Select(s => new GetSongDto
        {
            Title = s.Title,
            Artist = s.Artist,
            Album = s.Album
        }).ToList();
    }

    public async Task<List<GetSongDto>> GetSongsByYear(int year)
    {
        var songs = await context.Songs
            .Where(s => s.Year == year)
            .ToListAsync();
        return songs.Select(s => new GetSongDto
        {
            Title = s.Title,
            Artist = s.Artist,
            Year = s.Year
        }).ToList();
    }

    public async Task<List<GetSongDto>> GetSongsByGenre(string genre)
    {
        var songs = await context.Songs
            .Where(s => s.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase))
            .ToListAsync();
        return songs.Select(s => new GetSongDto
        {
            Title = s.Title,
            Artist = s.Artist,
            Genre = s.Genre
        }).ToList();
    }

    public async Task<List<GetSongDto>> GetSongsByTag(string tag)
    {
        var songs = await context.Songs
            .Where(s => s.Tags.Contains(tag, StringComparison.OrdinalIgnoreCase)) // Figure out how to search a list within a list
            .ToListAsync();
        return songs.Select(s => new GetSongDto
        {
            Title = s.Title,
            Artist = s.Artist,
            Tags = s.Tags
        }).ToList();
    }

    public async Task<PostSongDto> AddSong(Song song)
    {
        context.Songs.Add(song);
        await context.SaveChangesAsync();
        return new PostSongDto
        {
            Id = song.Id,
            Title = song.Title,
            Artist = song.Artist,
            Album = song.Album,
            Year = song.Year,
            Genre = song.Genre,
            Tags = song.Tags
        };
    }

    public async Task<bool> UpdateSong(int id, Song song)
    {
        var existingSong = await context.Songs.FindAsync(id);
        if (existingSong == null) return false;

        existingSong.Title = song.Title;
        existingSong.Artist = song.Artist;
        existingSong.Album = song.Album;
        existingSong.Year = song.Year;
        existingSong.Genre = song.Genre;
        existingSong.Tags = song.Tags;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteSong(int id)
    {
        var existingSong = await context.Songs.FindAsync(id);
        if (existingSong == null) return false;

        context.Songs.Remove(existingSong);
        await context.SaveChangesAsync();
        return true;
    }
}*/