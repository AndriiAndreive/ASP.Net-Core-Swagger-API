using Microsoft.EntityFrameworkCore;

namespace MovieAPI.Models
{
    public class DataSeeder
    {
        private readonly IServiceProvider _serviceProvider;

        public DataSeeder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Seed()
        {
            using var scope = _serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            var dbContext = services.GetRequiredService<MovieDbContext>();

            if (dbContext.Database.GetPendingMigrations().Any())
            {
                dbContext.Database.Migrate();
            }

            // Check if data already exists
            if (dbContext.Movies.Any() || dbContext.Actors.Any() || dbContext.MovieRatings.Any())
            {
                return; // Data has already been seeded
            }

            // Data seeding
            dbContext.Actors.AddRange(
                new Actor { Id = 1, Name = "Oksana Shalovynska" },
                new Actor { Id = 2, Name = "Jim Faulhaber" },
                new Actor { Id = 3, Name = "Farhood Sani" },
                new Actor { Id = 4, Name = "John Crane" },
                new Actor { Id = 5, Name = "Connor Wells" },
                new Actor { Id = 6, Name = "Richard Paul" },
                new Actor { Id = 7, Name = "Austin Holmes" },
                new Actor { Id = 8, Name = "Jerry Lynn" },
                new Actor { Id = 9, Name = "Freddy Blade" }
            );
            dbContext.Movies.AddRange(
                new Movie { Id = 1, Title = "The Spectacular Adventures of Sam", Actors = "1,2,3" },
                new Movie { Id = 2, Title = "Journey to the Forgotten Land", Actors = "2,3,4,6,8" },
                new Movie { Id = 3, Title = "Mystery of the Hidden Treasure", Actors = "1,2" },
                new Movie { Id = 4, Title = "A Tale of Two Worlds", Actors = "4,5,8,7,6" },
                new Movie { Id = 5, Title = "Escape from Destiny's Grip", Actors = "1,3,4,8,9" }
            );
            dbContext.MovieRatings.AddRange(
                new MovieRating { Id = 1, Rating = 3, MovieId = 1 },
                new MovieRating { Id = 2, Rating = 5, MovieId = 2 },
                new MovieRating { Id = 3, Rating = 4, MovieId = 3 },
                new MovieRating { Id = 4, Rating = 1, MovieId = 4 }
            );
            dbContext.SaveChanges();
        }
    }
}
