using Microsoft.EntityFrameworkCore;
using SongList.API.Models;

namespace SongList.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Song> Songs => Set<Song>();
}