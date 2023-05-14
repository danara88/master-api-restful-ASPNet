using ApiMovies.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiMovies.Data
{
    /// <summary>
    /// Application DB context
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Add your models here ...
        public DbSet<Category> Category { get; set; }

        public DbSet<Movie> Movie { get; set; }

        public DbSet<User> User { get; set; }
    }
}
