using SongList.Console.Models;

bool exit = false;
while (!exit)
{
    Console.WriteLine("Menu:");
    Console.WriteLine("1. List all songs");
    Console.WriteLine("2. Search songlist by...");
    Console.WriteLine("3. Add a new song");
    Console.WriteLine("4. Update an existing song");
    Console.WriteLine("5. Delete a song");
    Console.WriteLine("6. Exit");
    Console.Write("Enter your choice (1-6): ");
    string? choice = Console.ReadLine();
    switch (choice)
    {
        case "1":
            ListAllSongs();
            break;
        case "2":
            SearchSongList();
            break;
        case "3":
            AddNewSong();
            break;
        case "4":
            UpdateExistingSong();
            break;
        case "5":
            DeleteSong();
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

List<Song> ListAllSongs()
{
    // Implement logic to list all songs
    Console.WriteLine("Listing all songs...");
    return new List<Song>();
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