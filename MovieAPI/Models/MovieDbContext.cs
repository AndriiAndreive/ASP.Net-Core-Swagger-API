using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace MovieAPI.Models
{
    /// <summary>
    /// Represents the database context for the application.
    /// </summary>
    public class MovieDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovieDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
        {
        }

        // DbSet properties represent database tables
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<MovieRating> MovieRatings { get; set; }

        /// <summary>
        /// Overrides the configuration of the model that was defined using Fluent API.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actor>().ToTable("Actors");
            modelBuilder.Entity<Movie>().ToTable("Movies");
            modelBuilder.Entity<MovieRating>().ToTable("MovieRatings");
        }

    }
}
