using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using SongList.Console.Models;

//HttpClient client = new HttpClient();
//client.BaseAddress = new Uri("http://localhost:5298");
//HttpResponseMessage response;

bool exit = false;
while (!exit)
{
    Console.WriteLine();
    Console.WriteLine("Menu:");
    Console.WriteLine("1. List all songs");
    Console.WriteLine("2. Search songlist by Id");
    Console.WriteLine("3. Add a new song");
    Console.WriteLine("4. Update an existing song");
    Console.WriteLine("5. Delete a song");
    Console.WriteLine("6. Exit");
    Console.Write("Enter your choice (1-6): ");
    string? choice = Console.ReadLine();
    switch (choice)
    {
        case "1":
            /*response = await client.GetAsync("/songs");
            if(response.IsSuccessStatusCode)
            {
                var songsRaw = await response.Content.ReadAsStringAsync();
                var songs = JsonSerializer.Deserialize<List<Song>>(
                    songsRaw,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                foreach(var song in songs)
                {
                    Console.WriteLine($"Id: {song.Id}");
                    Console.WriteLine($"Title: {song.Title}");
                    Console.WriteLine($"Artist: {song.Artist}");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }*/
            var songs = await GetAllSongs();

            foreach(var song in songs)
            {
                Console.WriteLine($"Id: {song.Id}");
                Console.WriteLine($"Title: {song.Title}");
                Console.WriteLine($"Artist: {song.Artist}");
                Console.WriteLine();
            }

            break;
        case "2":
            /*Console.WriteLine("Enter search Id: ");
            string? searchIdString = Console.ReadLine();
            if(searchIdString != null)
            {
                int id = int.Parse(searchIdString);
                response = await client.GetAsync($"/songs/{id}");
                if(response.IsSuccessStatusCode)
                {
                    var searchSongRaw = await response.Content.ReadAsStringAsync();
                    var searchSong = JsonSerializer.Deserialize<Song>(
                        searchSongRaw,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
                    
                    string tagsString = string.Join(",", searchSong.Tags);
                    
                    Console.WriteLine($"Id: {searchSong.Id}");
                    Console.WriteLine($"Title: {searchSong.Title}");
                    Console.WriteLine($"Artist: {searchSong.Artist}");
                    Console.WriteLine($"Album: {searchSong.Album}");
                    Console.WriteLine($"Year: {searchSong.Year}");
                    Console.WriteLine($"Genre: {searchSong.Genre}");
                    Console.WriteLine($"Tags: {tagsString}");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }*/
            var searchSong = await GetSongById();

            string tagsString = string.Join(",", searchSong.Tags);
            
            Console.WriteLine($"Id: {searchSong.Id}");
            Console.WriteLine($"Title: {searchSong.Title}");
            Console.WriteLine($"Artist: {searchSong.Artist}");
            Console.WriteLine($"Album: {searchSong.Album}");
            Console.WriteLine($"Year: {searchSong.Year}");
            Console.WriteLine($"Genre: {searchSong.Genre}");
            Console.WriteLine($"Tags: {tagsString}");
            Console.WriteLine();

            break;
        case "3":
            /*Console.WriteLine("Enter new song information.");

            //Console.WriteLine("Id: ");
            //int newId = int.Parse(Console.ReadLine());

            Console.WriteLine("Title: ");
            string? newTitle = Console.ReadLine();

            Console.WriteLine("Artist: ");
            string? newArtist = Console.ReadLine();

            Console.WriteLine("Album");
            string? newAlbum = Console.ReadLine();

            Console.WriteLine("Year: ");
            int newYear = int.Parse(Console.ReadLine());

            Console.WriteLine("Genre: ");
            string? newGenre = Console.ReadLine();

            Console.WriteLine("Tags(Seperated by commas): ");
            List<string> newTags = Console.ReadLine().Split(",").ToList();

            response = await client.GetAsync("/songs");
            if(response.IsSuccessStatusCode)
            {
                var currentSongsRaw = await response.Content.ReadAsStringAsync();
                var currentSongs = JsonSerializer.Deserialize<List<Song>>(
                    currentSongsRaw,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                Song newSong = new Song
                {
                    Id = currentSongs.Count + 1,
                    Title = newTitle,
                    Artist = newArtist,
                    Album = newAlbum,
                    Year = newYear,
                    Genre = newGenre,
                    Tags = newTags
                };

                string newSongRaw = JsonSerializer.Serialize(newSong);
                var stringContent = new StringContent(newSongRaw, Encoding.UTF8, "application/json");
                await client.PostAsync("/songs", stringContent);
                Console.WriteLine($"Created song with id: {newSong.Id}");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }*/
            await AddNewSong();

            break;
        case "4":
            /*Console.WriteLine("Enter id of song to update: ");
            string? updateIdString = Console.ReadLine();
            if(updateIdString != null)
            {
                int updateId = int.Parse(updateIdString);
                response = await client.GetAsync($"/songs/{updateId}");
                if(response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var updatedSong = JsonSerializer.Deserialize<Song>(
                        data,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
                    Console.WriteLine("Enter new information. Leave blank to keep existing information.");
                    
                    Console.WriteLine($"Current title: {updatedSong.Title}");
                    Console.WriteLine("New title: ");
                    string? updateTitle = Console.ReadLine();
                    if(!updateTitle.IsWhiteSpace())
                    {
                        updatedSong.Title = updateTitle;
                    }

                    Console.WriteLine($"Current artist: {updatedSong.Artist}");
                    Console.WriteLine("New artist: ");
                    string? updateArtist = Console.ReadLine();
                    if(!updateArtist.IsWhiteSpace())
                    {
                        updatedSong.Artist = updateArtist;
                    }

                    Console.WriteLine($"Current album: {updatedSong.Album}");
                    Console.WriteLine("New album: ");
                    string? updateAlbum = Console.ReadLine();
                    if(!updateAlbum.IsWhiteSpace())
                    {
                        updatedSong.Album = updateAlbum;
                    }

                    Console.WriteLine($"Current year: {updatedSong.Year}");
                    Console.WriteLine("New year: ");
                    string? updateYearString = Console.ReadLine();
                    if(!updateYearString.IsWhiteSpace())
                    {
                        int updateYear = int.Parse(updateYearString);
                        updatedSong.Year = updateYear;
                    }

                    Console.WriteLine($"Current genre: {updatedSong.Genre}");
                    Console.WriteLine("New genre: ");
                    string? updateGenre = Console.ReadLine();
                    if(!updateGenre.IsWhiteSpace())
                    {
                        updatedSong.Genre = updateGenre;
                    }

                    string currentTagsString = string.Join(",", updatedSong.Tags);
                    Console.WriteLine($"Current tags: {currentTagsString}");
                    Console.WriteLine("New tags (Seperated by commas): ");
                    string? updateTagsString = Console.ReadLine();
                    if(!updateTagsString.IsWhiteSpace())
                    {
                        List<string> updateTags = updateTagsString.Split(",").ToList();
                        updatedSong.Tags = updateTags;
                    }

                    string updatedJson = JsonSerializer.Serialize(updatedSong);
                    var updatedStringContent = new StringContent(updatedJson, Encoding.UTF8, "application/json");
                    await client.PutAsync($"/songs/{updateId}", updatedStringContent);
                    Console.WriteLine($"Updated song with id: {updatedSong.Id}");
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
            else
            {
                Console.WriteLine("Invalid choice");
            }*/
            await UpdateExistingSong();

            break;
        case "5":
            /*Console.WriteLine("Enter id of song to be deleted: ");
            string? deleteIdString = Console.ReadLine();
            if(deleteIdString != null)
            {
                int deleteId = int.Parse(deleteIdString);
                response = await client.DeleteAsync($"/songs/{deleteId}");
                if(response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Deleted song with Id: {deleteId}");
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }*/
            await DeleteSong();
            break;
        case "6":
            exit = true;
            Console.WriteLine("Exiting the program. Goodbye!");
            break;
        default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
    }
}

async Task<List<Song>> GetAllSongs()
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

async Task<Song> GetSongById()
{
    using (var client = new HttpClient())
    {
        client.BaseAddress = new Uri("http://localhost:5298");
        Console.WriteLine("Enter search Id: ");
        string? searchIdString = Console.ReadLine();
        if (int.TryParse(searchIdString, out int searchId))
        {
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
                    return await GetSongById();
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return await GetSongById();
            }
        }
        else
        {
            Console.WriteLine("Invalid response. Please enter a number.");
            return await GetSongById();
        }
    }
}

async Task AddNewSong()
{
    using (var client = new HttpClient())
    {
        client.BaseAddress = new Uri("http://localhost:5298");
        var songList = await GetAllSongs();
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
            return;
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
            };

            string newSongRaw = JsonSerializer.Serialize(newSong);
            var stringContent = new StringContent(newSongRaw, Encoding.UTF8, "application/json");
            await client.PostAsync("/songs", stringContent);
            Console.WriteLine($"Created song with id: {newSong.Id}");
            return;
        }
        else
        {
            Console.WriteLine("Please enter valid year.");
            await AddNewSong();
            return;
        }
    }
}

async Task UpdateExistingSong()
{
    using (var client = new HttpClient())
    {
        client.BaseAddress = new Uri("http://localhost:5298");
        var updateSong = await GetSongById();

        Console.WriteLine("Enter new information. Leave blank to keep existing information.");
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
        }

        string updateJson = JsonSerializer.Serialize(updateSong);
        var updateStringContent = new StringContent(updateJson, Encoding.UTF8, "application/json");
        await client.PutAsync($"/songs/{updateSong.Id}", updateStringContent);
        Console.WriteLine($"Updated song with id: {updateSong.Id}");
        return;
    }
}

async Task DeleteSong()
{
    using (var client = new HttpClient())
    {
        client.BaseAddress = new Uri("http://localhost:5298");
        Console.WriteLine("Enter id of song to be deleted: ");
        string? deleteIdString = Console.ReadLine();
        if(int.TryParse(deleteIdString, out int deleteId))
        {
            HttpResponseMessage response = await client.DeleteAsync($"/songs/{deleteId}");
            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Deleted song with Id: {deleteId}");
                return;
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return;
            }
        }
        else
        {
            Console.WriteLine("Invalid Id.");
            await DeleteSong();
        }
    }
}