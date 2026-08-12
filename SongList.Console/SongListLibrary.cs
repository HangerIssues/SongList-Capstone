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
            
            /*foreach(var song in songs)
            {
                Console.WriteLine($"Id: {song.Id}");
                Console.WriteLine($"Title: {song.Title}");
                Console.WriteLine($"Artist: {song.Artist}");
                Console.WriteLine();
            }*/

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
            //Console.WriteLine("Enter search Id: ");
            //string? searchIdString = Console.ReadLine();
            //if (int.TryParse(searchIdString, out int searchId))
            //{
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
            /*}
            else
            {
                Console.WriteLine("Invalid response. Please enter a number.");
                return await GetSongById();
            }*/
        }
    }

    public async Task<bool> AddNewSong(Song newSong)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("http://localhost:5298");
            /*var songList = await GetAllSongs();
            Console.WriteLine("Enter new song information.");
            Console.WriteLine("Title: ");
            string? newTitle = Console.ReadLine();

            Console.WriteLine("Artist: ");
            string? newArtist = Console.ReadLine();
        
            Console.WriteLine("Album: ");
            string? newAlbum = Console.ReadLine();

            Console.WriteLine("Year: ");
            string? newYearString = Console.ReadLine();

            Console.WriteLine("Genre: ");
            string? newGenre = Console.ReadLine();

            Console.WriteLine("Tags (Seperate by commas): ");
            string? newTagsString = Console.ReadLine();

            if(string.IsNullOrWhiteSpace(newTitle) || string.IsNullOrWhiteSpace(newArtist) || string.IsNullOrWhiteSpace(newAlbum) || string.IsNullOrWhiteSpace(newYearString) || string.IsNullOrWhiteSpace(newGenre) || string.IsNullOrWhiteSpace(newTagsString))
            {
                Console.WriteLine("Please provide all fields.");
                await AddNewSong();
                return true;
            }
            else if(int.TryParse(newYearString, out int newYear))
            {
                List<string> newTags = newTagsString.Split(",").ToList();
                Song newSong = new Song
                {
                    Id = songList.Count + 1,
                    Title = newTitle,
                    Artist = newArtist,
                    Album = newAlbum,
                    Year = newYear,
                    Genre = newGenre,
                    Tags = newTags
                };*/

                string newSongRaw = JsonSerializer.Serialize(newSong);
                var stringContent = new StringContent(newSongRaw, Encoding.UTF8, "application/json");
                await client.PostAsync("/songs", stringContent);
                Console.WriteLine($"Created song with id: {newSong.Id}");
                return true;
            /*}
            else
            {
                Console.WriteLine("Please enter valid year.");
                await AddNewSong();
                return true;
            }*/
        }
    }

    public async Task<bool> UpdateExistingSong(int searchId, Song updateSong)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("http://localhost:5298");
            //var updateSong = await GetSongById(searchId);

            /*Console.WriteLine("Enter new information. Leave blank to keep existing information.");
            Console.WriteLine($"Current Title: {updateSong.Title}");
            Console.WriteLine("New Title: ");
            string? updateTitle = Console.ReadLine();
            if(!string.IsNullOrWhiteSpace(updateTitle))
            {
                updateSong.Title = updateTitle;
            }

            Console.WriteLine($"Current Artist: {updateSong.Artist}");
            Console.WriteLine("New Artist: ");
            string? updateArtist = Console.ReadLine();
            if(!string.IsNullOrWhiteSpace(updateArtist))
            {
                updateSong.Artist = updateArtist;
            }

            Console.WriteLine($"Current Album: {updateSong.Album}");
            Console.WriteLine("New Album: ");
            string? updateAlbum = Console.ReadLine();
            if(!string.IsNullOrWhiteSpace(updateAlbum))
            {
                updateSong.Album = updateAlbum;
            }

            Console.WriteLine($"Current Year: {updateSong.Year}");
            Console.WriteLine("New Year: ");
            string? updateYearString = Console.ReadLine();
            if(int.TryParse(updateYearString, out int updateYear))
            {
                updateSong.Year = updateYear;
            }

            Console.WriteLine($"Current Genre: {updateSong.Genre}");
            Console.WriteLine("New Genre: ");
            string? updateGenre = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(updateGenre))
            {
                updateSong.Genre = updateGenre;
            }

            string currentTagsString = string.Join(",", updateSong.Tags);
            Console.WriteLine($"Current Tags: {currentTagsString}");
            Console.WriteLine("New Tags (Seperate by commas): ");
            string? updateTagsString = Console.ReadLine();
            if(!string.IsNullOrWhiteSpace(updateTagsString))
            {
                List<string> updateTags = updateTagsString.Split(",").ToList();
                updateSong.Tags = updateTags;
            }*/

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
            /*Console.WriteLine("Enter id of song to be deleted: ");
            string? deleteIdString = Console.ReadLine();
            if(int.TryParse(deleteIdString, out int deleteId))
            {*/
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
            /*}
            else
            {
                Console.WriteLine("Invalid Id.");
                await DeleteSong();
                return true;
            }*/
        }
    }
}