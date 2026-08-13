using SongList.Console.Models;
using System.Text.Json;
using System.Text;

namespace SongListLibrary;

public class MethodLibrary
{
    public async Task<List<Song>> GetAllSongs()
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("http://localhost:5298");
            HttpResponseMessage response = await client.GetAsync("/songs");
            if(response.IsSuccessStatusCode)
            {
                var songsRaw = await response.Content.ReadAsStringAsync();
                var songs = JsonSerializer.Deserialize<List<Song>>(
                    songsRaw,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if(songs != null)
                {
                    return songs;
                }
                else
                {
                    Console.WriteLine("No Songs Found.");
                    return new List<Song>();
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return new List<Song>();
            }
        }
    }

    public async Task<Song> GetSongById(int searchId)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("http://localhost:5298");
            HttpResponseMessage response = await client.GetAsync($"/songs/{searchId}");
            if(response.IsSuccessStatusCode)
            {
                var searchSongRaw = await response.Content.ReadAsStringAsync();
                var searchSong = JsonSerializer.Deserialize<Song>(
                    searchSongRaw,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
                
                if(searchSong != null)
                {
                    return searchSong;
                }
                else
                {
                    Console.WriteLine($"Song with id {searchId} not found.");
                    var songs = await GetAllSongs();
                    Console.WriteLine($"Id to search cannot be less than 1 or greater than {songs.Count}.");
                    return await GetSongById(searchId);
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return await GetSongById(searchId);
            }
        }
    }

    public async Task<bool> AddNewSong(Song newSong)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("http://localhost:5298");

            string newSongRaw = JsonSerializer.Serialize(newSong);
            var stringContent = new StringContent(newSongRaw, Encoding.UTF8, "application/json");
            await client.PostAsync("/songs", stringContent);
            Console.WriteLine($"Created song with id: {newSong.Id}");
            return true;
        }
    }

    public async Task<bool> UpdateExistingSong(int searchId, Song updateSong)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("http://localhost:5298");

            string updateJson = JsonSerializer.Serialize(updateSong);
            var updateStringContent = new StringContent(updateJson, Encoding.UTF8, "application/json");
            await client.PutAsync($"/songs/{updateSong.Id}", updateStringContent);
            Console.WriteLine($"Updated song with id: {updateSong.Id}");
            return true;
        }
    }

    public async Task<bool> DeleteSong(int deleteId)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("http://localhost:5298");
            HttpResponseMessage response = await client.DeleteAsync($"/songs/{deleteId}");
            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Deleted song with Id: {deleteId}");
                return true;
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return false;
            }
        }
    }
}