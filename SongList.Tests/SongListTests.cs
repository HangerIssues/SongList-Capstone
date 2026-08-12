using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongList.Console.Models;
using SongListLibrary;

namespace SongList.Tests;

[TestClass]
public sealed class SongListTests
{
    [TestMethod]
    public async Task GetAllSongs_Connection_ReturnSongList()
    {
        var methodLibrary = new MethodLibrary();
        var result = await methodLibrary.GetAllSongs();
        Assert.IsNotEmpty(result);
    }

    [TestMethod]
    public async Task GetSongById_SongExists_ReturnSong()
    {
        var methodLibrary = new MethodLibrary();
        int searchId = 1;

        var result = await methodLibrary.GetSongById(searchId);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task AddNewSong_SongCreated_ReturnTrue()
    {
        var methodLibrary = new MethodLibrary();
        var songList = await methodLibrary.GetAllSongs();
        Song newSong = new Song
        {
            Id = songList.Count + 1,
            Title = "Test Title",
            Artist = "Test Artist",
            Album = "Test Album",
            Year = 2000,
            Genre = "Test Genre",
            Tags = {"test1","test2","test3"}
        };

        var result = await methodLibrary.AddNewSong(newSong);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task UpdateExistingSong_SongUpdated_ReturnTrue()
    {
        var methodLibrary = new MethodLibrary();
        int updateId = 2;
        Song updateSong = await methodLibrary.GetSongById(updateId);
        
        var result = await methodLibrary.UpdateExistingSong(updateId, updateSong);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task DeleteSong_SongExists_ReturnTrue()
    {
        var methodLibrary = new MethodLibrary();
        int deleteId = 3;
        
        var result = await methodLibrary.DeleteSong(deleteId);

        Assert.IsTrue(result);
    }
}
