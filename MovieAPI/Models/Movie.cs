namespace MovieAPI.Models
{
    /// <summary>
    /// Represents a movie entity.
    /// </summary>
    public class Movie
    {
        /// <summary>
        /// Gets or sets the unique identifier for the movie.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the movie.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the collection of ids for actors associated with the movie.
        /// </summary>
        public string? Actors { get; set; }
    }
}
