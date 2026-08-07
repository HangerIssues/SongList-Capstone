using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using SongList.Console.Models;

HttpClient client = new HttpClient();
client.BaseAddress = new Uri("http://localhost:5298");
HttpResponseMessage response;

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
            response = await client.GetAsync("/songs");
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
            }
            break;
        case "2":
            Console.WriteLine("Enter search Id: ");
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
            }
            break;
        case "3":
            Console.WriteLine("Enter new song information.");

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
            }

            break;
        case "4":
            Console.WriteLine("Enter id of song to update: ");
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
            }

            break;
        case "5":
            Console.WriteLine("Enter id of song to be deleted: ");
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
            }
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
/*
async void ListAllSongs()
{
    using (var client = new HttpClient())
    {
        client.BaseAddress = new Uri("http://localhost:5298");
        HttpResponseMessage response = await client.GetAsync("/songs");
        if(response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            Console.WriteLine(data);
            return;
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
            return;
        }
    }
}

List<Song> SearchSongList()
{
    // Implement logic to search the song list
    Console.WriteLine("Searching song list...");
    return new List<Song>();
}

void AddNewSong()
{
    // Implement logic to add a new song
    Console.WriteLine("Adding a new song...");
}

void UpdateExistingSong()
{
    // Implement logic to update an existing song
    Console.WriteLine("Updating an existing song...");
}

void DeleteSong()
{
    // Implement logic to delete a song
    Console.WriteLine("Deleting a song...");
}*/