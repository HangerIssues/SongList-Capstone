using SongList.API.Models;
using SongList.API.Dtos;

namespace SongList.API.Services;

public interface ISongListService
{
    Task<List<GetSongDto>> GetAllSongs();
    Task<GetSongDto> GetSongById(int id);
    Task<List<GetSongDto>> GetSongsByTitle(string title);
    Task<List<GetSongDto>> GetSongsByArtist(string artist);
    Task<List<GetSongDto>> GetSongsByAlbum(string album);
    Task<List<GetSongDto>> GetSongsByYear(int year);
    Task<List<GetSongDto>> GetSongsByGenre(string genre);
    Task<List<GetSongDto>> GetSongsByTag(string tag);
    Task<PostSongDto> AddSong(Song song);
    Task<bool> UpdateSong(int id, Song song);
    Task<bool> DeleteSong(int id);
}