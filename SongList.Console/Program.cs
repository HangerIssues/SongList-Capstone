using System.Text.Json;
using SongList.Console.Models;

HttpClient client = new HttpClient();

client.BaseAddress = new Uri("http://localhost:5298");
HttpResponseMessage response;

bool exit = false;
while (!exit)
{
    Console.WriteLine("Menu:");
    Console.WriteLine("1. Get all songs");
    Console.WriteLine("2. Get song(s) by...");
    Console.WriteLine("3. Add a new song");
    Console.WriteLine("4. Update a song");
    Console.WriteLine("5. Delete a song");
    Console.WriteLine("6. Exit");
    Console.WriteLine();
    Console.Write("Enter your choice (1-6): ");
    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            response = await client.GetAsync("/api/songs");
            Console.WriteLine("All songs:");
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            break;
        case "2":
            Console.WriteLine("Search by:");
            Console.WriteLine("1. Id");
            Console.WriteLine("2. Title");
            Console.WriteLine("3. Artist");
            Console.WriteLine("4. Album");
            Console.WriteLine("5. Year");
            Console.WriteLine("6. Genre");
            Console.WriteLine("7. Tag");
            Console.WriteLine();
            Console.Write("Enter your choice (1-7): ");
            string? searchChoice = Console.ReadLine();
            string? searchEndpoint = null;
            switch (searchChoice)
            {
                case "1":
                    searchEndpoint = "/api/songs/{id}";
                    break;
                case "2":
                    searchEndpoint = "/api/songs/{title}";
                    break;
                case "3":
                    searchEndpoint = "/api/songs/{artist}";
                    break;
                case "4":
                    searchEndpoint = "/api/songs/{album}";
                    break;
                case "5":
                    searchEndpoint = "/api/songs/{year}";
                    break;
                case "6":
                    searchEndpoint = "/api/songs/{genre}";
                    break;
                case "7":
                    searchEndpoint = "/api/songs/{tag}";
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    continue;
            }

            if (searchEndpoint != null)
            {
                Console.Write("Enter the search value: ");
                int searchValue = int.Parse(Console.ReadLine() ?? "0");
                response = await client.GetAsync($"{searchEndpoint}/{searchValue}");
                Console.WriteLine($"Search results for {searchEndpoint} with value {searchValue}:");
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice.");
                continue;
            }
        case "3":
            Console.WriteLine("Enter the details of the new song:");
            Console.Write("Title: ");
            string? title = Console.ReadLine();
            Console.Write("Artist: ");
            string? artist = Console.ReadLine();
            Console.Write("Album: ");
            string? album = Console.ReadLine();
            Console.Write("Year: ");
            int year = int.Parse(Console.ReadLine() ?? "0");
            Console.Write("Genre: ");
            string? genre = Console.ReadLine();
            Console.Write("Tags (comma-separated): ");
            string? tagsInput = Console.ReadLine();
            List<string> tags = tagsInput?.Split(',').Select(t => t.Trim()).ToList() ?? new List<string>();

            var newSong = new Song
            {
                Title = title,
                Artist = artist,
                Album = album,
                Year = year,
                Genre = genre,
                Tags = tags
            };

            if (string.IsNullOrWhiteSpace(newSong.Title) || string.IsNullOrWhiteSpace(newSong.Artist) || string.IsNullOrWhiteSpace(newSong.Album) || string.IsNullOrWhiteSpace(newSong.Genre))
            {
                Console.WriteLine("Error: Title, Artist, Album, and Genre are required fields.");
                continue;
            }
            
            var newJsonContent = JsonSerializer.Serialize(newSong);
            var newHttpContent = new StringContent(newJsonContent, System.Text.Encoding.UTF8, "application/json");
            response = await client.PostAsync("/api/songs", newHttpContent);
            break;
        case "4":
            Console.WriteLine("Which song would you like to update? (Enter the ID)");
            string? songId = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(songId))
            {
                Console.WriteLine("Error: Song ID is required.");
                continue;
            }
            var songs = JsonSerializer.Deserialize<List<Song>>(
                json: await client.GetStringAsync("/api/songs"),
                options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var song = songs.FirstOrDefault(s => s.Id == int.Parse(songId));
            if (song == null)
            {
                Console.WriteLine("Error: Song not found.");
                continue;
            }

            Console.WriteLine("Enter the new details of the song (leave blank to keep current value):");
            Console.Write($"Title ({song.Title}): ");
            string? newTitle = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newTitle))
            {
                song.Title = newTitle;
            }

            Console.Write($"Artist ({song.Artist}): ");
            string? newArtist = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newArtist))
            {
                song.Artist = newArtist;
            }

            Console.Write($"Album ({song.Album}): ");
            string? newAlbum = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newAlbum))
            {
                song.Album = newAlbum;
            }

            Console.Write($"Year ({song.Year}): ");
            string? newYearInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newYearInput))
            {
                song.Year = int.Parse(newYearInput);
            }

            Console.Write($"Genre ({song.Genre}): ");
            string? newGenre = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newGenre))
            {
                song.Genre = newGenre;
            }

            var updatedJsonContent = JsonSerializer.Serialize(song);
            var updatedHttpContent = new StringContent(updatedJsonContent, System.Text.Encoding.UTF8, "application/json");
            response = await client.PutAsync($"/api/songs/{songId}", updatedHttpContent);
            break;
        case "5":
            Console.WriteLine("Which song would you like to delete? (Enter the ID)");
            string? deleteSongId = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(deleteSongId))
            {
                Console.WriteLine("Error: Song ID is required.");
                continue;
            }
            response = await client.DeleteAsync($"/api/songs/{deleteSongId}");
            break;
        case "6":
            exit = true;
            continue;
        default:
            Console.WriteLine("Invalid choice.");
            continue;
    }
}
/*
if (response.IsSuccessStatusCode)
{
    string jsonResponse = await response.Content.ReadAsStringAsync();

    var songs = JsonSerializer.Deserialize<List<Song>>(
        json: jsonResponse,
        options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    
    foreach (var song in songs)
    {
        Console.WriteLine($"{song.Title} by {song.Artist}");
    }
}
else
{
    Console.WriteLine($"Error: {response.StatusCode}");
    Console.WriteLine(await response.Content.ReadAsStringAsync());
}

HttpResponseMessage singleResponse = await client.GetAsync("/api/songs/1");

if (response.IsSuccessStatusCode)
{
    string jsonResponse = await singleResponse.Content.ReadAsStringAsync();

    var song = JsonSerializer.Deserialize<Song>(
        json: jsonResponse,
        options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
    );

    Console.WriteLine($"{song.Title} by {song.Artist}");
}
else
{
    Console.WriteLine($"Error: {singleResponse.StatusCode}");
    Console.WriteLine(await singleResponse.Content.ReadAsStringAsync());
}
*/