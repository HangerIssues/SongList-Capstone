# SongList-Capstone
## Purpose
The SongList-Capstone project is intended to help organize songs by adding custom tags to each entry. These can include anything from what musical era the song is from to what message it sends.

## Instructions
To build and run, move to the folder that contains the .sln file and all three project files within a terminal. Enter the following command:
```bash
dotnet run --project SongList.API/SongList.API.csproj
```
Then, in the same file location on a second terminal, enter this command:
```bash
dotnet run --project SongList.Console/SongList.Console.csproj
```

## Unit Testing
To run the automated tests, use the first provided command to run the API, then run each test individually. Do not run all tests at the same time.

## What I Learned
While going through Code:You and making this project, I learned a lot. Some things were so new to me I didn't even know I didn't know them. I learned how to debug using actual tools instead of constant print statements. Asynchronous processes have been very useful. I struggled with APIs but managed to make this project work with one but intend to improve on it as I go.

## Plans
If I had more time on this project, I would have made the API controller-based and able to handle much more functionality. At its current state, song entries are only sorted and searchable by their ID, but I planned to include the ability to search and filter by each property. I also would have liked to make some kind of UI so the user end isn't just plain text in the console. I would've liked to add those features before the deadline, but I do plan to add them afterward as I continue to work on this and other projects.

### AI Note
Some AI was used but only to type repetitive lines. Those few sections are marked in the code.
