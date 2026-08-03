using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SongList.API.Models;

namespace SongList.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongController : ControllerBase
    {
        private List<Song> Songs { get; } = new List<Song>();

        public SongController()
        {
            string jsonFile = System.IO.File.ReadAllText("./Resources/SongList.json");
            var songData = JsonSerializer.Deserialize<List<Song>>(
                json: jsonFile,
                options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            if (songData != null)
            {
                Songs = songData;
            }
        }

        // GET: api/songs
        [HttpGet]
        public ActionResult<List<Song>> GetSongs()
        {
            return Ok(Songs);
        }

        //GET: api/songs/{id}
        [HttpGet("{id}")]
        public ActionResult<Song> GetById(int id)
        {
            var result = Songs.FirstOrDefault(s => s.Id == id);

            if (result == null)
            {
                return NotFound();
            }
            
            return Ok(result);
        }

        // GET: api/songs/{title}
        [HttpGet("{title}")]
        public ActionResult<Song> GetByTitle([FromRoute] string title)
        {
            var results = Songs.Where(s => s.Title.Equals(title, StringComparison.OrdinalIgnoreCase)).ToList();
            if (results == null || results.Count == 0)
            {
                return NotFound();
            }

            return Ok(results);
        }

        // GET: api/songs/search?artist={artist}
        [HttpGet("search/artist")]
        public ActionResult<Song> GetByArtist([FromQuery] string artist)
        {
            var results = Songs.Where(s => s.Artist.Equals(artist, StringComparison.OrdinalIgnoreCase)).ToList();
            if (results == null || results.Count == 0)
            {
                return NotFound();
            }

            return Ok(results);
        }

        // GET: api/songs/search?album={album}
        [HttpGet("search/album")]
        public ActionResult<Song> GetByAlbum([FromQuery] string album)
        {
            var results = Songs.Where(s => s.Album.Equals(album, StringComparison.OrdinalIgnoreCase)).ToList();
            if (results == null || results.Count == 0)
            {
                return NotFound();
            }

            return Ok(results);
        }

        // GET: api/songs/search?year={year}
        [HttpGet("search/year")]
        public ActionResult<Song> GetByYear([FromQuery] int year)
        {
            var results = Songs.Where(s => s.Year == year).ToList();
            if (results == null || results.Count == 0)
            {
                return NotFound();
            }

            return Ok(results);
        }

        // GET: api/songs/search?genre={genre}
        [HttpGet("search/genre")]
        public ActionResult<Song> GetByGenre([FromQuery] string genre)
        {
            var results = Songs.Where(s => s.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase)).ToList();
            if (results == null || results.Count == 0)
            {
                return NotFound();
            }

            return Ok(results);
        }

        // GET: api/songs/search?tag={tag}
        [HttpGet("search/tag")]
        public ActionResult<Song> GetByTag([FromQuery] string tag)
        {
            var results = Songs.Where(s => s.Tags.Any(t => t.Equals(tag, StringComparison.OrdinalIgnoreCase))).ToList();
            if (results == null || results.Count == 0)
            {
                return NotFound();
            }

            return Ok(results);
        }

        // POST: api/songs
        [HttpPost]
        public ActionResult<Song> CreateSong([FromBody] Song song)
        {
            Songs.Add(song);
            return CreatedAtAction(nameof(GetById), new { id = song.Id }, song);
        }

        // PUT: api/songs/{id}
        [HttpPut("{id}")]
        public ActionResult<Song> UpdateSong(int id, [FromBody] Song updatedSong)
        {
            var existingSong = Songs.FirstOrDefault(s => s.Id == id);
            if (existingSong == null)
            {
                return NotFound();
            }

            existingSong.Title = updatedSong.Title ?? existingSong.Title;
            existingSong.Artist = updatedSong.Artist ?? existingSong.Artist;
            existingSong.Album = updatedSong.Album ?? existingSong.Album;
            existingSong.Year = updatedSong.Year != 0 ? updatedSong.Year : existingSong.Year;
            existingSong.Genre = updatedSong.Genre ?? existingSong.Genre;
            existingSong.Tags = updatedSong.Tags ?? existingSong.Tags;

            return Ok(existingSong);
        }

        // DELETE: api/songs/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteSong(int id)
        {
            var songToDelete = Songs.FirstOrDefault(s => s.Id == id);
            if (songToDelete == null)
            {
                return NotFound();
            }

            Songs.Remove(songToDelete);
            return NoContent();
        }
    }
}