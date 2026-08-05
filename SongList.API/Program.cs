using Scalar.AspNetCore;
using SongList.API.Services;
using SongList.API.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
/*
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
*/
// builder.Services.AddScoped<ISongListService, SongListService>();

var app = builder.Build();

string jsonFile = File.ReadAllText("./Resources/SongList.json");
var jsonData = JsonSerializer.Deserialize<List<Song>>(
    json: jsonFile,
    options: new JsonSerializerOptions {PropertyNameCaseInsensitive = true});

// Get all songs
app.MapGet(pattern: "/songs", handler: () => jsonData)
    .WithName("GetAllSongs")
    .WithOpenApi()
    .Produces<List<Song>>(statusCode: StatusCodes.Status200OK);

// Get song by Id
app.MapGet(pattern: "/songs/{id}", handler: async (int id) =>
{
    var song = jsonData?.FirstOrDefault(s => s.Id == id);
    if (song == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(song);
})
    .WithName("GetSongById")
    .WithOpenApi()
    .Produces<Song>(statusCode: StatusCodes.Status200OK)
    .Produces(statusCode: StatusCodes.Status404NotFound);

// Add a new song
app.MapPost(pattern: "/songs", handler: (Song song) =>
{
    if (jsonData == null)
    {
        return Results.BadRequest();
    }

    jsonData.Add(song);

    var updatedJson = JsonSerializer.Serialize(jsonData, new JsonSerializerOptions {WriteIndented = true });
    File.WriteAllText("./Resources/SongList.json", updatedJson);
    return Results.Created($"/songs/{song.Id}", song);
})
    .WithName("AddSong")
    .WithOpenApi()
    .Produces<Song>(statusCode: StatusCodes.Status201Created)
    .Produces(statusCode: StatusCodes.Status400BadRequest);

// Update an existing song
app.MapPut(pattern: "/songs/{id}", handler: (int id, Song song) =>
{
    var updatedSong = jsonData?.FirstOrDefault(s => s.Id == id);
    if (updatedSong == null)
    {
        return Results.NotFound();
    }

    updatedSong.Id = song.Id;
    updatedSong.Title = song.Title;
    updatedSong.Artist = song.Artist;
    updatedSong.Album = song.Album;
    updatedSong.Year = song.Year;
    updatedSong.Genre = song.Genre;
    updatedSong.Tags = song.Tags;

    var updatedJson = JsonSerializer.Serialize(jsonData, new JsonSerializerOptions {WriteIndented = true });
    File.WriteAllText("./Resources/SongList.json", updatedJson);
    return Results.Ok(updatedSong);
})
    .WithName("UpdateSong")
    .WithOpenApi()
    .Produces<Song>(statusCode: StatusCodes.Status200OK)
    .Produces(statusCode: StatusCodes.Status404NotFound)
    .Produces(statusCode: StatusCodes.Status400BadRequest);

// Delete a song
app.MapDelete(pattern: "/songs/{id}", handler: (int id) =>
{
    var deletedSong = jsonData?.FirstOrDefault(s => s.Id == id);
    if(deletedSong == null)
    {
        return Results.NotFound();
    }

    jsonData?.Remove(deletedSong);

    var updatedJson = JsonSerializer.Serialize(jsonData, new JsonSerializerOptions {WriteIndented = true });
    File.WriteAllText("./Resources/SongList.json", updatedJson);
    return Results.NoContent();
})
    .WithName("DeleteSong")
    .WithOpenApi()
    .Produces(statusCode: StatusCodes.Status204NoContent)
    .Produces(statusCode: StatusCodes.Status404NotFound);

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.Run();
