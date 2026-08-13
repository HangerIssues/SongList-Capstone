using SongList.Console.Models;
using SongListLibrary;

var methodLibrary = new MethodLibrary();
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
            var songs = await methodLibrary.GetAllSongs();

            foreach(var song in songs)
            {
                Console.WriteLine($"Id: {song.Id}");
                Console.WriteLine($"Title: {song.Title}");
                Console.WriteLine($"Artist: {song.Artist}");
                Console.WriteLine();
            }

            break;
        case "2":
            Console.WriteLine("Enter searchId: ");
            if(!int.TryParse(Console.ReadLine(), out int searchId))
            {
                Console.WriteLine("Invalid id.");
                break;
            }
            var searchSong = await methodLibrary.GetSongById(searchId);

            string tagsString = string.Join(",", searchSong.Tags);
            //AI used to autocomplete repetitive lines
            Console.WriteLine($"Id: {searchSong.Id}");
            Console.WriteLine($"Title: {searchSong.Title}");
            Console.WriteLine($"Artist: {searchSong.Artist}");
            Console.WriteLine($"Album: {searchSong.Album}");
            Console.WriteLine($"Year: {searchSong.Year}");
            Console.WriteLine($"Genre: {searchSong.Genre}");
            Console.WriteLine($"Tags: {tagsString}");
            Console.WriteLine();
            //End of autocomplete
            break;
        case "3":
            var songList = await methodLibrary.GetAllSongs();
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
                break;
                
            }
            if(int.TryParse(newYearString, out int newYear))
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
                await methodLibrary.AddNewSong(newSong);
                break;
            }

            break;
        case "4":
            Console.WriteLine("Enter searchId: ");
            if(!int.TryParse(Console.ReadLine(), out int updateId))
            {
                Console.WriteLine("Invalid id.");
                break;
            }
            var updateSong = await methodLibrary.GetSongById(updateId);

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
            await methodLibrary.UpdateExistingSong(updateId, updateSong);

            break;
        case "5":
            Console.WriteLine("Enter id of song to be deleted: ");
            if(!int.TryParse(Console.ReadLine(), out int deleteId))
            {
                Console.WriteLine("Invalid id.");
                break;
            }
            await methodLibrary.DeleteSong(deleteId);
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