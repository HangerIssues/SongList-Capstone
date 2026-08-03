namespace SongList.Console.Models
{
    public class Song
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Artist { get; set; }
        public required string Album { get; set; }
        public int Year { get; set; }
        public required string Genre { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}