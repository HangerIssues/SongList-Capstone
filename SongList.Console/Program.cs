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
                var data = await response.Content.ReadAsStringAsync();
                Console.WriteLine(data);
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
                    var data = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(data);
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
            Console.WriteLine("Enter song information.");

            Console.WriteLine("Id: ");
            int newId = int.Parse(Console.ReadLine());

            Console.WriteLine("Title: ");
            string? title = Console.ReadLine();

            Console.WriteLine("Artist: ");
            string? artist = Console.ReadLine();

            Console.WriteLine("Album");
            string? album = Console.ReadLine();

            Console.WriteLine("Year: ");
            int year = int.Parse(Console.ReadLine());

            Console.WriteLine("Genre: ");
            string? genre = Console.ReadLine();

            Console.WriteLine("Tags(Seperated by commas): ");
            List<string> tags = Console.ReadLine().Split(",").ToList();

            Song song = new Song
            {
                Id = newId,
                Title = title,
                Artist = artist,
                Album = album,
                Year = year,
                Genre = genre,
                Tags = tags
            };

            string json = JsonSerializer.Serialize(song);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            response = await client.PostAsync("/songs", stringContent);

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
                    Song? updatedSong = JsonSerializer.Deserialize<Song>(data);
                    Console.WriteLine("Enter new information. Leave blank to keep existing information.");
                    Console.WriteLine("New title: ");
                    string? newTitle = Console.ReadLine();
                    if(newTitle != null)
                    {
                        updatedSong.Title = newTitle;
                    }

                    Console.WriteLine("New artist: ");
                    string? newArtist = Console.ReadLine();
                    if(newArtist != null)
                    {
                        updatedSong.Artist = newArtist;
                    }

                    Console.WriteLine("New album: ");
                    string? newAlbum = Console.ReadLine();
                    if(newAlbum != null)
                    {
                        updatedSong.Album = newAlbum;
                    }

                    Console.WriteLine("New year: ");
                    string? newYearString = Console.ReadLine();
                    if(newYearString != null)
                    {
                        int newYear = int.Parse(newYearString);
                        updatedSong.Year = newYear;
                    }

                    Console.WriteLine("New genre: ");
                    string? newGenre = Console.ReadLine();
                    if(newGenre != null)
                    {
                        updatedSong.Genre = newGenre;
                    }

                    Console.WriteLine("New tags (Seperated by commas): ");
                    string? newTagsString = Console.ReadLine();
                    if(newTagsString != null)
                    {
                        List<string> newTags = newTagsString.Split(",").ToList();
                        updatedSong.Tags = newTags;
                    }

                    string updatedJson = JsonSerializer.Serialize(updatedSong);
                    var updatedStringContent = new StringContent(updatedJson, Encoding.UTF8, "application/json");
                    response = await client.PutAsync($"/songs/{updateId}", updatedStringContent);
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
            string? idString = Console.ReadLine();
            if(idString != null)
            {
                int deleteId = int.Parse(idString);
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
}