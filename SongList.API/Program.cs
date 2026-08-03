using System.Text.Json;
using SongList.API.Models;
using Microsoft.AspNetCore.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

var app = builder.Build();

string jsonFile = File.ReadAllText(path: "./Resources/SongList.json");
var jsonData = JsonSerializer.Deserialize<List<Song>>(
    json: jsonFile,
    options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

// Get all songs
app.MapGet(pattern: "/api/songs", handler: () => jsonData)
    .WithName(endpointName: "GetSongs")
    .AddOpenApiOperationTransformer((operation, context, cancellationToken) =>
    {
        operation.Summary = "Get all songs";
        operation.Description = "Returns a list of all songs in the database.";
        return Task.CompletedTask;
    })
    .Produces<List<Song>>(statusCode: StatusCodes.Status200OK);

// Get song by ID
app.MapGet(pattern: "/api/songs/{id}", handler: (int id) => jsonData)
    .WithName(endpointName: "GetById")
    .AddOpenApiOperationTransformer((operation, context, cancellationToken) =>
    {
        operation.Summary = "Get a song by ID";
        operation.Description = "Returns the matching song based on the provided ID.";
        return Task.CompletedTask;
    })
    .Produces<Song>(statusCode: StatusCodes.Status200OK)
    .Produces(statusCode: StatusCodes.Status404NotFound);

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
